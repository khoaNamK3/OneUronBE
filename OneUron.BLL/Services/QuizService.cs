using FluentValidation;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.DTOs.QuestionDTOs;
using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.DTOs.SubjectDTOs;
using OneUron.BLL.DTOs.UserQuizAttemptDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository;
using OneUron.DAL.Repository.QuizRepo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;
        private readonly IQuestionService _questionService;
        private readonly IValidator<QuizRequestDto> _quizRequestValidator;
        private readonly IUserQuizAttemptService _userQuizAttemptService;
        private readonly IProcessService _processService;
        public QuizService(
            IQuizRepository quizRepository,
            IQuestionService questionService,
            IValidator<QuizRequestDto> quizRequestValidator,
            IUserQuizAttemptService userQuizAttemptService,
            IProcessService processService)
        {
            _quizRepository = quizRepository;
            _questionService = questionService;
            _quizRequestValidator = quizRequestValidator;
            _userQuizAttemptService = userQuizAttemptService;
            _processService = processService;
        }


        public async Task<List<QuizResponseDto>> GetAllQuizAsync()
        {
            var quizzes = await _quizRepository.GetAllQuizAsync();

            if (quizzes == null || !quizzes.Any())
                throw new ApiException.NotFoundException("Không tìm thấy bài kiểm tra ");

            return quizzes.Select(MapToDTO).ToList();
        }


        public async Task<QuizResponseDto> GetQuizByIdAsync(Guid id)
        {
            var quiz = await _quizRepository.GetQuizByIdAsync(id);

            if (quiz == null)
                throw new ApiException.NotFoundException($"Bài kiểm tra của ID {id} không tìm thấy.");

            return MapToDTO(quiz);
        }

        public async Task<PagedResult<QuizResponseDto>> GetPagedQuizzesAsync(int pageNumber, int pageSize, string? name)
        {
            var pagedResult = await _quizRepository.GetPagedQuizzesAsync(pageNumber, pageSize, name);

            var resposne = pagedResult.Items.Select(MapToDTO).ToList();

            return new PagedResult<QuizResponseDto>
            {
                CurrentPage = pagedResult.CurrentPage,
                PageSize = pagedResult.PageSize,
                TotalCount = pagedResult.TotalCount,
                Items = resposne
            };
        }

        public async Task<UserQuizInformationResponse> GetUserQuizInformation(Guid userId)
        {
            var existQuizzes = await _quizRepository.GetAllQuizByUserId(userId);

            if (existQuizzes == null || !existQuizzes.Any())
                throw new ApiException.NotFoundException("Người dùng không có bài kiểm tra ");

            int totalCompleteQuiz = 0;
            int totalQuizPassed = 0;
            double totalMinute = 0;
            double totalPoints = 0;
            int totalAttempts = 0;
            int totalQuizWatting = 0;

            foreach (var quiz in existQuizzes)
            {
                var attempts = quiz.UserQuizAttempts?.ToList() ?? new List<UserQuizAttempt>();
                if (!attempts.Any())
                {
                    totalQuizWatting++;
                    continue; // bỏ qua quiz nếu ko có attemp
                }
                totalCompleteQuiz++;

                foreach (var attempt in attempts)
                {
                   
                    if (attempt.FinishAt > attempt.StartAt)
                    {
                        totalMinute += (attempt.FinishAt - attempt.StartAt).TotalMinutes;
                    }

                   
                    totalPoints += attempt.Point;
                    totalAttempts++;

                   
                    if (attempt.Point >= quiz.PassScore)
                    {
                        totalQuizPassed++;
                    }
                }
            }

            double averageScore = totalAttempts > 0
                ? totalPoints / totalAttempts
                : 0;

            return new UserQuizInformationResponse
            {
                TotalCompleteQuiz = totalCompleteQuiz,
                TotalQuizPassed = totalQuizPassed,
                NumberQuizWaitting = totalQuizWatting,
                TotalTime = Math.Round(totalMinute, 8),
                AverageScore = Math.Round(averageScore, 2)
            };
        }


        public async Task<QuizResponseDto> CreateNewQuizAsync(QuizRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Bài kiểm tra mới không được để trống.");

            var validationResult = await _quizRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newQuiz = MaptoEntity(request);
            await _quizRepository.AddAsync(newQuiz);

            return MapToDTO(newQuiz);
        }


        public async Task<QuizResponseDto> UpdateQuizByIdAsync(Guid id, QuizRequestDto request)
        {
            var existingQuiz = await _quizRepository.GetQuizByIdAsync(id);
            if (existingQuiz == null)
                throw new ApiException.NotFoundException($"Bài kiểm tra của  ID {id} Không tìm thấy.");

            if (request == null)
                throw new ApiException.BadRequestException("Bài kiểm tra mới không được để trống");

            var validationResult = await _quizRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existingQuiz.Name = request.Name;
            existingQuiz.Description = request.Description;
            existingQuiz.TotalQuestion = request.TotalQuestion;
            existingQuiz.TotalPoints = request.TotalPoints;
            existingQuiz.Time = request.Time;
            existingQuiz.Type = request.Type;
            existingQuiz.PassScore = request.PassScore;
            existingQuiz.UserId = request.UserId;

            await _quizRepository.UpdateAsync(existingQuiz);

            return MapToDTO(existingQuiz);
        }


        public async Task<QuizResponseDto> DeleteQuizByIdAsync(Guid id)
        {
            var existingQuiz = await _quizRepository.GetByIdAsync(id);
            if (existingQuiz == null)
                throw new ApiException.NotFoundException($"Bài kiểm tra của  ID {id} Không tìm thấy.");

            await _quizRepository.DeleteAsync(existingQuiz);
            return MapToDTO(existingQuiz);
        }

        public async Task<UserScheduleInformationResponse> GetUserScheduleInformationAsync(Guid userId, Guid scheduleId)
        {
            // get all quiz of user
            var quizzes = await _quizRepository.GetAllQuizByUserId(userId);
            if (quizzes == null || !quizzes.Any())
                throw new ApiException.NotFoundException("Người dùng không có bài kiểm tra");

            double totalUserAttemp = quizzes.Sum(q => q.UserQuizAttempts?.Count ?? 0);

            // get all  process
            var processes = await _processService.GetProcessesByScheduleId(scheduleId);
            if (processes == null || !processes.Any())
                throw new ApiException.NotFoundException("Không tìm thấy quá trình của lịch học này .");

            var allTasks = processes
                .SelectMany(p => p.ProcessTasks ?? Enumerable.Empty<ProcessTaskResponseDto>())
                .ToList();

            // total task complete
            double totalTimeProcessTaskComplete = allTasks
                .Where(t => t.IsCompleted && t.EndTime > t.StartTime)
                .Sum(t => (t.EndTime - t.StartTime).TotalHours);

            // caculate  streak
            DateTime today = DateTime.UtcNow.Date;
            var recentProcesses = processes
                .Where(p => p.Date.Date >= today.AddDays(-2) && p.Date.Date <= today)
                .OrderBy(p => p.Date)
                .ToList();

            int streak = 0;
            int currentStreak = 0;
            DateTime prevDate = DateTime.MinValue;

            foreach (var process in recentProcesses)
            {
                bool hasCompletedTask = process.ProcessTasks?.Any(t => t.IsCompleted) ?? false;
                bool isConsecutive = prevDate == DateTime.MinValue || (process.Date.Date - prevDate).TotalDays == 1;

                if (hasCompletedTask && isConsecutive)
                    currentStreak++;
                else if (hasCompletedTask)
                    currentStreak = 1;
                else
                    currentStreak = 0;

                streak = Math.Max(streak, currentStreak);
                prevDate = process.Date.Date;
            }

            //  list task near complete 
            var nearProcessTaskComplete = recentProcesses
                .SelectMany(p => p.ProcessTasks ?? Enumerable.Empty<ProcessTaskResponseDto>())
                .Where(t => t.IsCompleted)
                .Select(t => new ProcessTaskResponseDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted = t.IsCompleted,
                    Note = t.Note,
                    StartTime = t.StartTime,
                    EndTime = t.EndTime,
                    ProcessId = t.ProcessId
                })
                .ToList();

            int numberTaskComplete = nearProcessTaskComplete.Count;

            //  Subject today and tomorrow
            var subjectFuture = processes
                .Where(p => p.Date.Date == today || p.Date.Date == today.AddDays(1))
                .SelectMany(p => p.Subjects ?? Enumerable.Empty<SubjectResponseDto>())
                .Select(s => new SubjectResponseDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Priority = s.Priority
                })
                .ToList();


            return new UserScheduleInformationResponse
            {
                TotalQuizUserAttemp = totalUserAttemp,
                TotalTimeProcessTaskComplete = totalTimeProcessTaskComplete,
                Streak = streak,
                NearProcessTaskComplete = nearProcessTaskComplete,
                NumbetTaskComplete = numberTaskComplete,
                SubjectFuture = subjectFuture
            };
        }




        public QuizResponseDto MapToDTO(Quiz quiz)
        {
            if (quiz == null) return null;

            return new QuizResponseDto
            {
                Id = quiz.Id,
                Name = quiz.Name,
                Description = quiz.Description,
                TotalQuestion = quiz.TotalQuestion,
                TotalPoints = quiz.TotalPoints,
                Time = quiz.Time,
                Type = quiz.Type,
                PassScore = quiz.PassScore,
                UserId = quiz.UserId,
                Questions = quiz.Questions?
                    .Select(q => _questionService.MapToDTO(q))
                    .ToList() ?? new List<QuestionResponseDto>()
            };
        }

        public Quiz MaptoEntity(QuizRequestDto newQuiz)
        {
            return new Quiz
            {
                Name = newQuiz.Name,
                Description = newQuiz.Description,
                TotalQuestion = newQuiz.TotalQuestion,
                TotalPoints = newQuiz.TotalPoints,
                Time = newQuiz.Time,
                Type = newQuiz.Type,
                PassScore = newQuiz.PassScore,
                UserId = newQuiz.UserId
            };
        }

        public async Task<PagedResult<QuizResponseDto>> GetAllQuizByUserIdAsync(int pageNumber, int pageSize, Guid userId)
        {
            var quizzes = await _quizRepository.GetAllQuizByUserIdAsync(pageNumber, pageSize, userId);

            if (!quizzes.Items.Any())
                throw new ApiException.NotFoundException("Không tìm thấy bài kiểm tra của người dùng này.");

            var result = quizzes.Items.Select(MapToDTO).ToList();

            return new PagedResult<QuizResponseDto>
            {
                CurrentPage = quizzes.CurrentPage,
                PageSize = quizzes.PageSize,
                TotalCount = quizzes.TotalCount,
                Items = result
            };
        }

    }
}
