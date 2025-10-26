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
        private readonly IUserRepository _userRepository;

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
            IUserRepository userRepository,
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
            _userRepository = userRepository;

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

            // calculate totalTime
            var totalDays = (scheduleSubject.EndDate - scheduleSubject.StartDate).TotalDays + 1;

            var  totalTime = $"{totalDays} ngày";

            // Count total AmountSubject

            var amountSubject = scheduleSubject.subjectListRequest?.Count ?? 0;

            var scheduleRequest = new ScheduleRequestDto
            {
                Title = scheduleSubject.Title,
                StartDate = scheduleSubject.StartDate,
                EndDate = scheduleSubject.EndDate,
                TotalTime = totalTime,
                AmountSubject = amountSubject,
                CreateAt = DateTime.UtcNow,
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
                    ScheduleId = schedule.Id
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
            var subjects = (await _subjectService.GetAllSubjectbyScheduleIdAsync(scheduleId)).ToList();
            var processes = (await _processService.GetProcessesByScheduleId(scheduleId)).ToList();

            if (!subjects.Any() || !processes.Any())
                return;

            var random = new Random();

            foreach (var processResponse in processes)
            {
                var processRequest = MapResponseToRequest(processResponse);


                var selectedSubjects = new List<SubjectResponseDto>();


                var baseSubject = subjects[random.Next(subjects.Count)];
                selectedSubjects.Add(baseSubject);


                foreach (var subject in subjects)
                {
                    if (selectedSubjects.Any(s => s.Id == subject.Id))
                        continue; // tránh duplicate

                    double chance = subject.Priority switch
                    {
                        SubjectType.High => 0.8,
                        SubjectType.Medium => 0.5,
                        SubjectType.Low => 0.25,
                        _ => 0.5
                    };

                    if (random.NextDouble() <= chance)
                    {
                        selectedSubjects.Add(subject);
                    }
                }


                processRequest.SubjectIds = selectedSubjects.Select(s => s.Id).ToList();


                await _processService.UpdateProcessByIdAsync(processResponse.Id, processRequest);
            }
        }

        private ProcessRequestDto MapResponseToRequest(ProcessResponseDto response)
        {
            return new ProcessRequestDto
            {
                Date = response.Date,
                Description = response.Description,
                ScheduleId = response.ScheduleId,
                SubjectIds = response.Subjects?.Select(s => s.Id).ToList() ?? new List<Guid>()
            };
        }

        public async Task<ProcessResponseDto> CreatProcessTaskForProcessAsync(Guid processId, ProcessTaskGenerateRequest taskGenerateRequest)
        {
            // ====== LẤY DỮ LIỆU ======
            var existProcess = await _processService.GetByIdAsync(processId)
                ?? throw new ApiException.NotFoundException("No Process Found");

            var existSchedule = await _scheduleService.GetByIdAsync(existProcess.ScheduleId)
                ?? throw new ApiException.NotFoundException("No Schedule Found");

            var existUser = await _userRepository.GetUserByUserIdAsync(existSchedule.UserId)
                ?? throw new ApiException.NotFoundException("No User Found");

            var existSubjects = await _subjectService.GetSubjectByProcessIdAsync(processId)
                ?? throw new ApiException.NotFoundException($"{processId} not found");

            var existStudyMethod = await _studyMethodService.GetByIdAsync(existUser.StudyMethod.Id)
                ?? throw new ApiException.NotFoundException("No studyMethod Found");

            var existMethod = await _methodSerivce.GetByIdAsync(existStudyMethod.MethodId)
                ?? throw new ApiException.NotFoundException("No Method Found");

            string listStringSubject = string.Join(", ", existSubjects.Select(s => s.Name));

            // ====== VALIDATION ======
            var validationTaskGenerate = await _processTaskGenerateValidator.ValidateAsync(taskGenerateRequest);
            if (!validationTaskGenerate.IsValid)
                throw new ApiException.ValidationException(validationTaskGenerate.Errors);

            // ====== PROMPT GỬI AI ======
            string prompt = $@"
        Bạn là hệ thống lập kế hoạch học tập thông minh.

        Hãy tạo danh sách task học tập theo phương pháp '{existMethod.Name}'.
        Thông tin:
        - Số lượng task cần tạo: {taskGenerateRequest.Amount}
        - Mô tả chi tiết: {taskGenerateRequest.Description}
        - Ngày học: {existProcess.Date:yyyy-MM-dd}
        - Danh sách môn học: {listStringSubject}

        Quy tắc thời gian:
        - Các task phải nối tiếp nhau trong cùng ngày {existProcess.Date:yyyy-MM-dd}.
        - Không được trùng hoặc chồng thời gian.
        - Toàn bộ thời gian phải nằm trong khoảng 06:00 → 22:00 cùng ngày.

        Định dạng bắt buộc:
        - Mỗi phần tử gồm: title, description, note, start_time, end_time.
        - Thời gian định dạng theo date của database postGreSQL
        - KHÔNG thêm mô tả, lời giải thích, ký tự lạ, hoặc văn bản ngoài JSON.

        Kết quả trả về DUY NHẤT:
        [
          {{
            ""title"": ""string"",
            ""description"": ""string"",
            ""note"": ""string"",
            ""start_time"": ""yyyy-MM-ddTHH:mm:ss"",
            ""end_time"": ""yyyy-MM-ddTHH:mm:ss""
          }}
        ]
    ";

          
            string aiResponse = await CallGeminiAsync(prompt);
            string cleanJson = NormalizeAiJson(aiResponse);

            var aiTasks = JsonSerializer.Deserialize<List<ProcessTaskResponseDto>>(cleanJson,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip
                }) ?? new List<ProcessTaskResponseDto>();

            
            var processTasks = new List<ProcessTaskResponseDto>();

            foreach (var task in aiTasks)
            {
                var newTask = new ProcessTaskRequestDto
                {
                    Note = task.Note,
                    Description = task.Description,
                    StartTime = task.StartTime,
                    EndTime = task.EndTime,
                    IsCompleted = false,
                    ProcessId = processId,
                    Title = task.Title
                };

                var created = await _processTaskService.CreateProcessTaskAsync(newTask);
                processTasks.Add(created);
            }

          
            await FixProcessTaskTimeIfMissingAsync(processId, taskGenerateRequest.StartTime);

          
            await AdjustProcessTaskTimeByMethodAsync(processId, taskGenerateRequest.StartTime);

            
            return await _processService.GetByIdAsync(processId);
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

        private async Task FixProcessTaskTimeIfMissingAsync(Guid processId, TimeOnly? startTime)
        {
            var process = await _processService.GetByIdAsync(processId)
                ?? throw new ApiException.NotFoundException("No Process Found");

            var processTasks = (await _processTaskService.GetAllProcessTaskByProcessIdAsync(processId)).ToList();
            if (!processTasks.Any()) return;

            var studyDuration = TimeSpan.FromMinutes(30);
            var breakDuration = TimeSpan.FromMinutes(10);

            var baseTime = process.Date.AddHours(startTime?.Hour ?? 8).AddMinutes(startTime?.Minute ?? 30);

            foreach (var task in processTasks.OrderBy(t => t.Title))
            {
                if (task.StartTime == DateTime.MinValue || task.EndTime == DateTime.MinValue)
                {
                    task.StartTime = baseTime;
                    task.EndTime = baseTime.Add(studyDuration);
                    baseTime = task.EndTime.Add(breakDuration);

                    await _processTaskService.UpdateProcessTaskByIdAsync(task.Id, MapResponseToRequest(task));
                }
            }
        }
        public async Task AdjustProcessTaskTimeByMethodAsync(Guid processId, TimeOnly startTime)
        {
            var process = await _processService.GetByIdAsync(processId)
                ?? throw new ApiException.NotFoundException("No Process Found");

            var schedule = await _scheduleService.GetByIdAsync(process.ScheduleId)
                ?? throw new ApiException.NotFoundException("No Schedule Found");

            var user = await _userRepository.GetUserByUserIdAsync(schedule.UserId)
                ?? throw new ApiException.NotFoundException("No User Found");

            var studyMethod = await _studyMethodService.GetByIdAsync(user.StudyMethod.Id)
                ?? throw new ApiException.NotFoundException("No StudyMethod Found");

            var method = await _methodSerivce.GetByIdAsync(studyMethod.MethodId)
                ?? throw new ApiException.NotFoundException("No Method Found");

            var processTasks = (await _processTaskService.GetAllProcessTaskByProcessIdAsync(processId)).ToList();
            if (!processTasks.Any())
                throw new ApiException.NotFoundException("No Process Tasks Found");

            int studyMinutes, breakMinutes;

            switch (method.Name.Trim().ToLower())
            {
                case "pomodoro": studyMinutes = 25; breakMinutes = 5; break;
                case "spaced repeation": studyMinutes = 20; breakMinutes = 10; break;
                case "active recall": studyMinutes = 30; breakMinutes = 10; break;
                case "mind mapping": studyMinutes = 45; breakMinutes = 15; break;
                case "feyman":
                case "feynman": studyMinutes = 60; breakMinutes = 10; break;
                default: studyMinutes = 30; breakMinutes = 5; break;
            }

            var currentStart = process.Date.AddHours(startTime.Hour).AddMinutes(startTime.Minute);

            foreach (var task in processTasks.OrderBy(t => t.StartTime))
            {
                var newStart = currentStart;
                var newEnd = newStart.AddMinutes(studyMinutes);

                if (newEnd.TimeOfDay > new TimeSpan(22, 0, 0))
                    break;

                task.StartTime = newStart;
                task.EndTime = newEnd;

                await _processTaskService.UpdateProcessTaskByIdAsync(task.Id, MapResponseToRequest(task));

                currentStart = newEnd.AddMinutes(breakMinutes);
            }
        }


        public ProcessTaskRequestDto MapResponseToRequest(ProcessTaskResponseDto response)
        {
            return new ProcessTaskRequestDto
            {
                Title = response.Title,
                Description = response.Description,
                Note = response.Note,
                StartTime = response.StartTime,
                EndTime = response.EndTime,
                IsCompleted = response.IsCompleted,
                ProcessId = response.ProcessId
            };
        }

    }
}
