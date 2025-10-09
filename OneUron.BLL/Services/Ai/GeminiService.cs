using Microsoft.Extensions.Configuration;
using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.DTOs.QuestionChoiceDTOs;
using OneUron.BLL.DTOs.QuestionDTOs;
using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
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
            IConfiguration configuration)
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


        public async Task<ApiResponse<QuizResponseDto>> GenerateQuestionByQuizIdAsync(QuizRequestDto newQuiz)
        {
            try
            {
                if (newQuiz == null)
                    return ApiResponse<QuizResponseDto>.FailResponse("GenerateQuestion Fail", "Quiz request is null");


                var createdQuiz = await _quizService.CreateNewQuizAsync(newQuiz);
                if (createdQuiz?.Data == null)
                    return ApiResponse<QuizResponseDto>.FailResponse("GenerateQuestion Fail", "Quiz not created");

                var quiz = createdQuiz.Data;


                var prompt = $"Tôi đang ôn về chủ đề {quiz.Name} và {quiz.Description}. " +
                             $"Bài kiểm tra có khoảng {quiz.TotalQuestion} câu hỏi, " +
                             $"thời gian làm bài là {quiz.Time}, độ khó ở mức {quiz.Type}, " +
                             $"tổng điểm là {quiz.TotalPoints}, và điểm qua môn là {quiz.PassScore}. " +
                             $"Hãy sinh ra các câu hỏi khác nhau phù hợp với các điều kiện trên, " +
                             $"và mỗi câu hỏi nên có các lựa chọn (answers) kèm đáp án đúng. " +
                             $"Trả về JSON đúng format sau, không có markdown hoặc ký tự ```:\n" +
                             @"{
                                 ""questions"": [
                                     {
                                         ""name"": ""Tên câu hỏi"",
                                         ""description"": ""Mô tả"",
                                         ""point"": 10,
                                         ""questionChoices"": [
                                             { ""name"": ""Đáp án 1"", ""isCorrect"": false },
                                             { ""name"": ""Đáp án 2"", ""isCorrect"": true }
                                         ]
                                     }
                                 ]
                               }";


                var aiResponse = await CallGeminiAsync(prompt);


                var quizWithQuestions = ParseQuestionsFromGemini(aiResponse, quiz);


                await SaveQuestionsToDatabaseAsync(quizWithQuestions);


                return ApiResponse<QuizResponseDto>.SuccessResponse(
                    quizWithQuestions,
                    "Generated and saved questions successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuizResponseDto>.FailResponse("GenerateQuestion Fail", ex.Message);
            }
        }


        private QuizResponseDto ParseQuestionsFromGemini(string jsonText, QuizResponseDto quiz)
        {
            try
            {

                jsonText = jsonText.Trim();
                if (jsonText.StartsWith("```"))
                {
                    int start = jsonText.IndexOf('{');
                    int end = jsonText.LastIndexOf('}');
                    if (start >= 0 && end > start)
                    {
                        jsonText = jsonText.Substring(start, end - start + 1);
                    }
                }


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

                    var choices = new List<QuestionChoiceReponseDto>();
                    foreach (var c in q.GetProperty("questionChoices").EnumerateArray())
                    {
                        choices.Add(new QuestionChoiceReponseDto
                        {
                            Id = Guid.NewGuid(),
                            Name = c.GetProperty("name").GetString(),
                            IsCorrect = c.GetProperty("isCorrect").GetBoolean()
                        });
                    }

                    question.QuestionChoices = choices;
                    questions.Add(question);
                }

                quiz.Questions = questions;
                return quiz;
            }
            catch (Exception ex)
            {
                throw new Exception($"Gemini response format invalid: {ex.Message}\nRaw response:\n{jsonText}");
            }
        }


        private async Task SaveQuestionsToDatabaseAsync(QuizResponseDto quiz)
        {
            foreach (var question in quiz.Questions)
            {

                var createdQuestion = await _questionService.CreateNewQuestionAsync(new QuestionRequestDto
                {
                    Name = question.Name ?? string.Empty,
                    Description = question.Description ?? string.Empty,
                    Point = question.Point,
                    QuizId = quiz.Id
                });

                if (createdQuestion?.Data == null)
                    continue;

                var questionId = createdQuestion.Data.Id;


                foreach (var choice in question.QuestionChoices)
                {
                    await _questionChoiceService.CreateNewQuestionChoiceAsync(new QuestionChoiceRequestDto
                    {
                        Name = choice.Name ?? string.Empty,
                        IsCorrect = choice.IsCorrect,
                        QuestionId = questionId
                    });
                }
            }
        }


        public async Task<ApiResponse<ScheduleResponeDto>> CreateTaskForScheduleFollowStudyMethodIdAsync(Guid studyMethodId, ScheduleRequestDto newSchedule)
        {
            try
            {

                if (newSchedule == null)
                    return ApiResponse<ScheduleResponeDto>.FailResponse("Create Task Fail", "Schedule request is null.");


                var scheduleResponse = await _scheduleService.CreateScheduleAsync(newSchedule);
                if (scheduleResponse?.Data == null)
                    return ApiResponse<ScheduleResponeDto>.FailResponse("Create Task Fail", "Failed to create schedule.");

                var schedule = scheduleResponse.Data;


                var startDate = schedule.StartDate.Date;
                var endDate = schedule.EndDate.Date;
                int totalDays = (endDate - startDate).Days + 1;

                var createdProcesses = new List<ProcessResponseDto>();

                for (int i = 0; i < totalDays; i++)
                {
                    var processDate = startDate.AddDays(i);

                    var processRequest = new ProcessRequestDto
                    {
                        Date = processDate,
                        Description = $"Process for {processDate:dd/MM/yyyy} in schedule {schedule.Title}",
                        ScheduleId = schedule.Id
                    };

                    var processResponse = await _processService.CreateProcessAsync(processRequest);
                    if (processResponse?.Data != null)
                        createdProcesses.Add(processResponse.Data);
                }


                var studyMethod = await _studyMethodService.GetByIdAsyc(studyMethodId);
                if (studyMethod?.Data == null)
                    return ApiResponse<ScheduleResponeDto>.FailResponse("Create Task Fail", "Study method not found.");

                var method = await _methodSerivce.GetByIdAsync(studyMethod.Data.MethodId);
                if (method?.Data == null)
                    return ApiResponse<ScheduleResponeDto>.FailResponse("Create Task Fail", "Method not found.");


                string promptTemplate =
                    $"Hãy tạo danh sách task học tập áp dụng phương pháp học '{method.Data.Name}'. " +
                    $"Số lượng task cần tạo là {schedule.AmountSubject} cho 1 ngày học (process). " +
                    $"Áp dụng phương pháp này dựa theo mô tả sau: {method.Data.Description}. " +
                    $"Hãy đảm bảo mỗi task có thời gian bắt đầu và kết thúc trong cùng 1 ngày. " +
                    "Trả về kết quả DUY NHẤT ở dạng JSON array, không kèm giải thích. " +
                    "Mỗi task phải có đầy đủ các field: " +
                    "[{\"title\":\"string\",\"description\":\"string\",\"note\":\"string\",\"startTime\":\"yyyy-MM-ddTHH:mm:ss\",\"endTime\":\"yyyy-MM-ddTHH:mm:ss\",\"isCompleted\":false}]";


                foreach (var process in createdProcesses)
                {
                   

                    string prompt = $"{promptTemplate}\n\nContext: Schedule = {schedule.Title}, Date = {process.Date:yyyy-MM-dd}";
                    var aiResponse = await CallGeminiAsync(prompt);

                    var cleanJson = aiResponse
                        .Trim()
                        .Replace("```json", "")
                        .Replace("```", "")
                        .Trim();

                    List<ProcessTaskRequestDto>? tasks = null;
                    try
                    {
                        tasks = JsonSerializer.Deserialize<List<ProcessTaskRequestDto>>(cleanJson,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                     
                    }
                    catch (Exception jsonEx)
                    {
                        Console.WriteLine($"[LOG  JSON parse error: {jsonEx.Message}\nOutput: {cleanJson}");
                    }

                    if (tasks?.Count > 0)
                    {
                        int index = 1;
                        foreach (var task in tasks)
                        {
                            task.ProcessId = process.Id;
                            task.IsCompleted = false;
                            task.Note ??= string.Empty;

                            task.StartTime = DateTime.SpecifyKind(task.StartTime, DateTimeKind.Utc);
                            task.EndTime = DateTime.SpecifyKind(task.EndTime, DateTimeKind.Utc);

                            var result = await _processTaskService.CreateProcessTaskAsync(task);

                           
                        }
                    }
                }
                var fullSchedule = await _scheduleService.GetByIdAsync(schedule.Id);
                return ApiResponse<ScheduleResponeDto>.SuccessResponse(fullSchedule.Data, "Schedule and AI-generated tasks created successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ScheduleResponeDto>.FailResponse("Create Task Fail", ex.Message);
            }
        }



    }
}
