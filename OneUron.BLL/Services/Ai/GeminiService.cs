using FluentValidation;
using Microsoft.Extensions.Configuration;
using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.DTOs.QuestionChoiceDTOs;
using OneUron.BLL.DTOs.QuestionDTOs;
using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.DTOs.SubjectDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.UserRepo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OneUron.BLL.Services.Ai
{
    public class GeminiService : IGeminiService
    {
        private readonly IQuizService _quizService;
        private readonly IQuestionService _questionService;
        private readonly IQuestionChoiceService _questionChoiceService;
        private readonly IMethodSerivce _methodSerivce;
        private readonly IStudyMethodService _studyMethodService;
        private readonly IScheduleService _scheduleService;
        private readonly IProcessService _processService;
        private readonly IProcessTaskService _processTaskService;
        private readonly ISubjectService _subjectService;

        private readonly IValidator<ScheduleSubjectRequestDto> _schedulerSubjectRequestValidator;
        private readonly IValidator<ProcessTaskGenerateRequest> _processTaskGenerateValidator;

        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly string _model;

        public GeminiService(
            IQuizService quizService,
            IQuestionService questionService,
            IQuestionChoiceService questionChoiceService,
            IMethodSerivce methodSerivce,
            IStudyMethodService studyMethodService,
            IScheduleService scheduleService,
            IProcessService processService,
            IProcessTaskService processTaskService,
            ISubjectService subjectService,
            IConfiguration configuration,
            IValidator<ScheduleSubjectRequestDto> schedulerSubjectRequestValidator,
             IValidator<ProcessTaskGenerateRequest> processTaskGenerateValidator
            )
        {
            _quizService = quizService;
            _questionService = questionService;
            _questionChoiceService = questionChoiceService;
            _methodSerivce = methodSerivce;
            _studyMethodService = studyMethodService;
            _scheduleService = scheduleService;
            _processService = processService;
            _processTaskService = processTaskService;
            _subjectService = subjectService;
            _schedulerSubjectRequestValidator = schedulerSubjectRequestValidator;
            _processTaskGenerateValidator = processTaskGenerateValidator;

            var quizKeyConfig = configuration.GetSection("QuizKey");
            _apiKey = quizKeyConfig["ApiKey"] ?? throw new ArgumentNullException("Missing Gemini API key");
            _baseUrl = quizKeyConfig["BaseUrl"] ?? "https://generativelanguage.googleapis.com";
            _model = quizKeyConfig["Model"] ?? "gemini-2.0-flash";
        }


        public async Task<string> CallGeminiAsync(string prompt)
        {
            using var httpClient = new HttpClient();

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var endpoint = $"{_baseUrl}/v1beta/models/{_model}:generateContent?key={_apiKey}";

            var response = await httpClient.PostAsJsonAsync(endpoint, requestBody);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>();
            var text = json
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text ?? string.Empty;
        }


        public async Task<QuizResponseDto> GenerateQuestionsByQuizAsync(QuizRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Quiz request cannot be null.");

            var createdQuiz = await _quizService.CreateNewQuizAsync(request);
            if (createdQuiz == null)
                throw new ApiException.BadRequestException("Quiz creation failed.");

            var quiz = createdQuiz;

            string prompt =
                $"Tôi đang ôn về chủ đề {quiz.Name} và {quiz.Description}. " +
                $"Bài kiểm tra có {quiz.TotalQuestion} câu, thời gian {quiz.Time}, độ khó {quiz.Type}, " +
                $"tổng điểm {quiz.TotalPoints}, điểm qua môn {quiz.PassScore}. " +
                "Sinh các câu hỏi khác nhau phù hợp, kèm đáp án đúng, trả về JSON (không có markdown) dạng: " +
                @"{ ""questions"": [ { ""name"": ""..."", ""description"": ""..."", ""point"": 10, ""questionChoices"": [ { ""name"": ""..."", ""isCorrect"": true } ] } ] }";

            var aiResponse = await CallGeminiAsync(prompt);
            var quizWithQuestions = ParseQuestions(aiResponse, quiz);
            await SaveQuestionsAsync(quizWithQuestions);

            return quizWithQuestions;
        }


        private QuizResponseDto ParseQuestions(string jsonText, QuizResponseDto quiz)
        {
            try
            {
                jsonText = CleanJson(jsonText);
                using var doc = JsonDocument.Parse(jsonText);
                var root = doc.RootElement.GetProperty("questions");

                var questions = new List<QuestionResponseDto>();

                foreach (var q in root.EnumerateArray())
                {
                    var question = new QuestionResponseDto
                    {
                        Id = Guid.NewGuid(),
                        Name = q.GetProperty("name").GetString(),
                        Description = q.GetProperty("description").GetString(),
                        Point = q.GetProperty("point").GetDouble(),
                        QuizId = quiz.Id
                    };

                    question.QuestionChoices = q.GetProperty("questionChoices")
                        .EnumerateArray()
                        .Select(c => new QuestionChoiceReponseDto
                        {
                            Id = Guid.NewGuid(),
                            Name = c.GetProperty("name").GetString(),
                            IsCorrect = c.GetProperty("isCorrect").GetBoolean()
                        })
                        .ToList();

                    questions.Add(question);
                }

                quiz.Questions = questions;
                return quiz;
            }
            catch (Exception ex)
            {
                throw new ApiException.BadRequestException($"Gemini response invalid: {ex.Message}");
            }
        }


        private static string CleanJson(string text)
        {
            text = text.Trim();
            if (text.StartsWith("```"))
            {
                int start = text.IndexOf('{');
                int end = text.LastIndexOf('}');
                if (start >= 0 && end > start)
                    text = text.Substring(start, end - start + 1);
            }
            return text.Replace("```json", "").Replace("```", "").Trim();
        }


        private async Task SaveQuestionsAsync(QuizResponseDto quiz)
        {
            foreach (var q in quiz.Questions)
            {
                var createdQuestion = await _questionService.CreateNewQuestionAsync(new QuestionRequestDto
                {
                    Name = q.Name ?? "",
                    Description = q.Description ?? "",
                    Point = q.Point,
                    QuizId = quiz.Id
                });

                if (createdQuestion == null) continue;
                var questionId = createdQuestion.Id;

                foreach (var choice in q.QuestionChoices)
                {
                    await _questionChoiceService.CreateNewQuestionChoiceAsync(new QuestionChoiceRequestDto
                    {
                        Name = choice.Name ?? "",
                        IsCorrect = choice.IsCorrect,
                        QuestionId = questionId
                    });
                }
            }
        }


        //public async Task<ScheduleResponeDto> CreateTasksForScheduleAsync(Guid studyMethodId, ScheduleRequestDto newSchedule)
        //{
        //    if (newSchedule == null)
        //        throw new ApiException.BadRequestException("Schedule request cannot be null.");

        //    var scheduleResponse = await _scheduleService.CreateScheduleAsync(newSchedule);
        //    if (scheduleResponse == null)
        //        throw new ApiException.BadRequestException("Failed to create schedule.");

        //    var schedule = scheduleResponse;
        //    var startDate = schedule.StartDate.Date;
        //    var endDate = schedule.EndDate.Date;
        //    int totalDays = (endDate - startDate).Days + 1;

        //    var processes = new List<ProcessResponseDto>();
        //    for (int i = 0; i < totalDays; i++)
        //    {
        //        var date = startDate.AddDays(i);
        //        var process = await _processService.CreateProcessAsync(new ProcessRequestDto
        //        {
        //            Date = date,
        //            Description = $"Process for {date:dd/MM/yyyy} in schedule {schedule.Title}",
        //            ScheduleId = schedule.Id
        //        });

        //        if (process != null)
        //            processes.Add(process);
        //    }

        //    var studyMethod = await _studyMethodService.GetByIdAsync(studyMethodId);
        //    if (studyMethod == null)
        //        throw new ApiException.NotFoundException("Study method not found.");

        //    var method = await _methodSerivce.GetByIdAsync(studyMethod.MethodId);
        //    if (method == null)
        //        throw new ApiException.NotFoundException("Method not found.");

        //    string promptTemplate =
        //        $"Tạo danh sách task học áp dụng phương pháp '{method.Name}' — mô tả: {method.Description}. " +
        //        $"Tạo {schedule.AmountSubject} task mỗi ngày. JSON array duy nhất dạng: " +
        //        @"[{""title"":""string"",""description"":""string"",""note"":""string"",""startTime"":""yyyy-MM-ddTHH:mm:ss"",""endTime"":""yyyy-MM-ddTHH:mm:ss"",""isCompleted"":false}]";

        //    foreach (var process in processes)
        //    {
        //        string prompt = $"{promptTemplate}\n\nContext: {schedule.Title} ({process.Date:yyyy-MM-dd})";
        //        var aiResponse = await CallGeminiAsync(prompt);
        //        var cleanJson = CleanJson(aiResponse);

        //        List<ProcessTaskRequestDto>? tasks;
        //        try
        //        {
        //            tasks = JsonSerializer.Deserialize<List<ProcessTaskRequestDto>>(cleanJson,
        //                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new ApiException.BadRequestException($"Invalid AI JSON for process {process.Id}: {ex.Message}");
        //        }

        //        if (tasks == null || tasks.Count == 0) continue;

        //        foreach (var task in tasks)
        //        {
        //            task.ProcessId = process.Id;
        //            task.IsCompleted = false;
        //            task.Note ??= string.Empty;

        //            task.StartTime = DateTime.SpecifyKind(task.StartTime, DateTimeKind.Utc);
        //            task.EndTime = DateTime.SpecifyKind(task.EndTime, DateTimeKind.Utc);

        //            await _processTaskService.CreateProcessTaskAsync(task);
        //        }
        //    }

        //    var fullSchedule = await _scheduleService.GetByIdAsync(schedule.Id);
        //    return fullSchedule ?? throw new ApiException.BadRequestException("Schedule retrieval failed after task creation.");
        //}


        public async Task<ScheduleResponeDto> CreateScheduleWithListSubjectAsync(ScheduleSubjectRequestDto scheduleSubject, Guid userId)
        {
            if (scheduleSubject == null)
                throw new ApiException.BadRequestException("New Data of scheduleSubject Request cannot be null");

            var validationResult = await _schedulerSubjectRequestValidator.ValidateAsync(scheduleSubject);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var scheduleRequest = new ScheduleRequestDto
            {
                Title = scheduleSubject.Title,
                StartDate = scheduleSubject.StartDate,
                EndDate = scheduleSubject.EndDate,
                TotalTime = scheduleSubject.TotalTime,
                AmountSubject = scheduleSubject.AmountSubject,
                CreateAt = scheduleSubject.CreateAt,
                UserId = userId
            };

            var schedule = await _scheduleService.CreateScheduleAsync(scheduleRequest)
                           ?? throw new ApiException.BadRequestException("Failed to create schedule");


            foreach (var subjectRequest in scheduleSubject.subjectListRequest)
            {
                var subject = new SubjectRequestDto
                {
                    Name = subjectRequest.Name,
                    Priority = subjectRequest.Priority,
                    ScheduleId = schedule.Id,
                    ProcessId = Guid.Empty
                };

                await _subjectService.CreateAsync(subject);
            }


            var existStudyMethod = await _studyMethodService.GetStudyMethodByUserIdAsync(userId)
                                   ?? throw new ApiException.NotFoundException("StudyMethod does not exist");

            var method = await _methodSerivce.GetByIdAsync(existStudyMethod.MethodId)
                         ?? throw new ApiException.NotFoundException("Method does not exist");


            string prompt = $@"
                        Bạn là AI lập kế hoạch học tập.

                        Nhiệm vụ: Tạo danh sách JSON các hoạt động học tập theo từng ngày, dựa trên:
                        - Phương pháp học: '{method.Name}'
                        - Mô tả: {method.Description}
                        - Khoảng thời gian học: từ {schedule.StartDate:yyyy-MM-dd} đến {schedule.EndDate:yyyy-MM-dd} (tuyệt đối không vượt ngoài khoảng này).

                        Yêu cầu:
                        - Mỗi ngày chỉ có 1 phần tử tương ứng với 1 ngày trong khoảng thời gian trên.
                        - Mỗi phần tử gồm: 
                          + date (định dạng yyyy-MM-dd, nằm trong khoảng từ {schedule.StartDate:yyyy-MM-dd} đến {schedule.EndDate:yyyy-MM-dd}),
                          + description (string, mô tả ngắn gọn hoạt động học tập),
                          + scheduleId (giá trị là null).
                        - Không thêm giải thích, không markdown, không ký tự thừa.
                        - Trả về duy nhất **một JSON array hợp lệ** như ví dụ:

                        [
                          {{ ""date"": ""{schedule.StartDate:yyyy-MM-dd}"", ""description"": ""Ôn bài A"", ""scheduleId"": null }},
                          {{ ""date"": ""{schedule.StartDate.AddDays(1):yyyy-MM-dd}"", ""description"": ""Học phần B"", ""scheduleId"": null }}
                        ]
                    ";

            var aiResponse = await CallGeminiAsync(prompt);
            var cleanJson = CleanJson(aiResponse);

            List<ProcessRequestDto>? generatedProcesses = null;

            try
            {
                var trimmed = cleanJson
                    .Trim()
                    .Replace("\\n", "")
                    .Replace("\\r", "")
                    .Replace("\\\"", "\"")
                    .Replace("```json", "")
                    .Replace("```", "")
                    .Trim();

                bool looksLikeArrayWithoutBrackets =
                    trimmed.StartsWith("{") && trimmed.Contains("},") && !trimmed.StartsWith("[");

                if (looksLikeArrayWithoutBrackets)
                {
                    trimmed = "[" + trimmed + "]";
                }

                if ((trimmed.StartsWith("\"") && trimmed.EndsWith("\"")) ||
                    (trimmed.StartsWith("'") && trimmed.EndsWith("'")))
                {
                    trimmed = trimmed.Substring(1, trimmed.Length - 2);
                }

                int startIndex = trimmed.IndexOf('[');
                int endIndex = trimmed.LastIndexOf(']');
                if (startIndex >= 0 && endIndex > startIndex)
                {
                    trimmed = trimmed.Substring(startIndex, endIndex - startIndex + 1);
                }

                generatedProcesses = JsonSerializer.Deserialize<List<ProcessRequestDto>>(
                    trimmed,
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        AllowTrailingCommas = true,
                        ReadCommentHandling = JsonCommentHandling.Skip
                    }
                );

                Console.WriteLine("Parsed JSON successfully:\n" + trimmed);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Raw AI Response:\n" + cleanJson);
                throw new ApiException.BadRequestException(
                    $"AI response parsing failed: {ex.Message}\n---\nRaw Cleaned JSON:\n{cleanJson}"
                );
            }

            if (generatedProcesses == null || !generatedProcesses.Any())
                throw new ApiException.BadRequestException("AI did not return any valid process.");


            generatedProcesses = generatedProcesses
                .Where(p => p.Date.Date >= schedule.StartDate.Date && p.Date.Date <= schedule.EndDate.Date)
                .ToList();

            if (!generatedProcesses.Any())
                throw new ApiException.BadRequestException("AI did not return any valid process within the schedule range.");

            foreach (var process in generatedProcesses)
            {
                process.ScheduleId = schedule.Id;
                await _processService.CreateProcessAsync(process);
            }

            await AssignSubjectToProcessAsync(schedule.Id);

            return await _scheduleService.GetByIdAsync(schedule.Id);
        }



        private async Task AssignSubjectToProcessAsync(Guid scheduleId)
        {
            var schedule = await _scheduleService.GetByIdAsync(scheduleId);
            var subjects = await _subjectService.GetAllSubjectbyScheduleIdAsync(scheduleId);
            var processes = await _processService.GetProcessesByScheduleId(scheduleId);

            if (!subjects.Any() || !processes.Any())
                return;

            var random = new Random();

            foreach (var subject in subjects)
            {
                double chance = subject.Priority switch
                {
                    SubjectType.High => 0.8,
                    SubjectType.Medium => 0.5,
                    SubjectType.Low => 0.25,
                    _ => 0.5
                };

                var orderedProcesses = processes
                    .OrderBy(p => random.NextDouble() + (subject.Priority == SubjectType.High ? -0.1 : 0.1))
                    .ToList();

                foreach (var process in orderedProcesses)
                {
                    if (random.NextDouble() <= chance)
                    {

                        var updateRequest = new SubjectRequestDto
                        {
                            Name = subject.Name,
                            Priority = subject.Priority,
                            ProcessId = process.Id,
                            ScheduleId = subject.ScheduleId
                        };

                        await _subjectService.UpdateByIdAsync(subject.Id, updateRequest);
                        break;
                    }
                }
            }
        }

        public async Task<ScheduleResponeDto> CreatProcessTaskForProcessAsync(Guid scheduleId, Guid userId, ProcessTaskGenerateRequest taskGenerateRequest)
        {

            var existSchedule = await _scheduleService.GetByIdAsync(scheduleId)
                ?? throw new ApiException.NotFoundException("Schedule does not exist.");

            var existStudyMethod = await _studyMethodService.GetStudyMethodByUserIdAsync(userId)
                ?? throw new ApiException.NotFoundException("StudyMethod does not exist.");

            var existMethod = await _methodSerivce.GetByIdAsync(existStudyMethod.MethodId)
                ?? throw new ApiException.NotFoundException("Method does not exist.");


            var processes = await _processService.GetProcessesByScheduleId(scheduleId);
            if (!processes.Any())
                throw new ApiException.NotFoundException("Schedule has no processes.");

            string allDates = string.Join(", ", processes.Select(p => p.Date.ToString("yyyy-MM-dd")));


            var validationTaskGenerate = await _processTaskGenerateValidator.ValidateAsync(taskGenerateRequest);
            if (!validationTaskGenerate.IsValid)
                throw new ApiException.ValidationException(validationTaskGenerate.Errors);


            string prompt = $@"
                Hãy tạo danh sách task học tập theo phương pháp '{existMethod.Name}'.
                Số lượng task: {taskGenerateRequest.Amount} cho mỗi ngày.
                Chi tiết: {taskGenerateRequest.Description}
                Ngày học: {allDates}

                Yêu cầu:
                - Thời gian bắt đầu của mỗi task phải lớn hơn hoặc bằng thời điểm hiện tại.
                - Thời gian kết thúc phải sau thời gian bắt đầu.
                - KHÔNG thêm ký tự ngoài JSON.
                - KHÔNG trả lời dưới dạng text, chỉ trả JSON duy nhất.
                - Chỉ trả về JSON duy nhất, định dạng:
                [
                  {{
                    ""title"": ""string"",
                    ""description"": ""string"",
                    ""start_time"": ""yyyy-MM-ddTHH:mm:ss"",
                    ""end_time"": ""yyyy-MM-ddTHH:mm:ss""
                  }}
                ]
                ";


            foreach (var process in processes)
            {
                string processContext = $@"
                    Context ngày học:
                    - Ngày: {process.Date:yyyy-MM-dd}
                    - Mô tả: {process.Description ?? "Không có mô tả"}";

                string finalPrompt = $"{prompt}\n{processContext}";

                var aiResponse = await CallGeminiAsync(finalPrompt);


                var cleanJson = NormalizeAiJson(aiResponse);

                List<ProcessTaskRequestDto> tasks;
                try
                {
                    tasks = JsonSerializer.Deserialize<List<ProcessTaskRequestDto>>(cleanJson,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                        ?? new List<ProcessTaskRequestDto>();
                }
                catch (Exception ex)
                {
                    throw new ApiException.BadRequestException(
                        $"Invalid AI JSON for process {process.Id} ({process.Date:yyyy-MM-dd}): {ex.Message}\nRaw JSON: {cleanJson}");
                }

                if (!tasks.Any()) continue;


                foreach (var task in tasks)
                {
                    task.ProcessId = process.Id;
                    task.IsCompleted = false;
                    task.Note ??= string.Empty;

                    task.StartTime = DateTime.SpecifyKind(task.StartTime, DateTimeKind.Utc);
                    task.EndTime = DateTime.SpecifyKind(task.EndTime, DateTimeKind.Utc);

                 
                    if (task.StartTime < DateTime.UtcNow)
                        task.StartTime = DateTime.UtcNow.AddMinutes(1);

           
                    if (task.EndTime <= task.StartTime)
                        task.EndTime = task.StartTime.AddMinutes(30);

                    await _processTaskService.CreateProcessTaskAsync(task);
                }
            }


            return await _scheduleService.GetByIdAsync(scheduleId)
                ?? throw new ApiException.BadRequestException("Failed to retrieve schedule after creating tasks.");
        }


        private string NormalizeAiJson(string aiResponse)
        {
            if (string.IsNullOrWhiteSpace(aiResponse))
                return "[]";

           
            var start = aiResponse.IndexOf('[');
            var end = aiResponse.LastIndexOf(']');

            if (start == -1 || end == -1 || end <= start)
                return "[]";

            string json = aiResponse[start..(end + 1)];

          
            json = Regex.Replace(json, @"\""start_time\"":\s*\""(.*?)\""", m =>
            {
                if (DateTime.TryParse(m.Groups[1].Value, out var dt))
                    return $"\"start_time\":\"{dt:yyyy-MM-ddTHH:mm:ss}\"";
                return m.Value;
            });

            json = Regex.Replace(json, @"\""end_time\"":\s*\""(.*?)\""", m =>
            {
                if (DateTime.TryParse(m.Groups[1].Value, out var dt))
                    return $"\"end_time\":\"{dt:yyyy-MM-ddTHH:mm:ss}\"";
                return m.Value;
            });

            return json;
        }

    }
}
