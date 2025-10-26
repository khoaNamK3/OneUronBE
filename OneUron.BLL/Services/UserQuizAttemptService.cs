using FluentValidation;
using OneUron.BLL.DTOs.AnswerDTOs;
using OneUron.BLL.DTOs.QuestionDTOs;
using OneUron.BLL.DTOs.UserAnswerDTOs;
using OneUron.BLL.DTOs.UserQuizAttemptDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository;
using OneUron.DAL.Repository.QuizRepo;
using OneUron.DAL.Repository.UserQuizAttemptRepo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class UserQuizAttemptService : IUserQuizAttemptService
    {
        private readonly IUserQuizAttemptReposiotry _userQuizAttemptRepository;
        private readonly IValidator<UserQuizAttemptRequestDto> _userQuizAttemptValidator;
        private readonly IAnswerService _answerService;
        private readonly IValidator<SubmitAnswerRequest> _submitAnswerRequestValidator;
        private readonly IQuestionService _questionService;
        private readonly IQuestionChoiceService _questionChoiceService;
        private readonly IQuizRepository _quizRepository;
        public UserQuizAttemptService(
            IUserQuizAttemptReposiotry userQuizAttemptRepository,
            IAnswerService answerService,
            IValidator<UserQuizAttemptRequestDto> userQuizAttemptValidator,
            IValidator<SubmitAnswerRequest> submitAnswerRequestValidator,
            IQuestionService questionService,
            IQuestionChoiceService questionChoiceService,
            IQuizRepository quizRepository)
        {
            _userQuizAttemptRepository = userQuizAttemptRepository;
            _answerService = answerService;
            _userQuizAttemptValidator = userQuizAttemptValidator;
            _submitAnswerRequestValidator = submitAnswerRequestValidator;
            _questionService = questionService;
            _questionChoiceService = questionChoiceService;
            _quizRepository = quizRepository;
        }


        public async Task<List<UserQuizAttemptResponseDto>> GetAllAsync()
        {
            var attempts = await _userQuizAttemptRepository.GetAllUserQuizAttemptAsync();

            if (attempts == null || !attempts.Any())
                throw new ApiException.NotFoundException("No quiz attempts found.");

            var result = await Task.WhenAll(attempts.Select(a => MapToDTO(a)));
            return result.ToList();
        }


        public async Task<UserQuizAttemptResponseDto> GetByIdAsync(Guid id)
        {
            var attempt = await _userQuizAttemptRepository.GetUserQuizAttemptsByIdAsync(id);
            if (attempt == null)
                throw new ApiException.NotFoundException($"Quiz attempt with ID {id} not found.");

            return await MapToDTO(attempt);
        }

        public async Task<List<UserQuizAttemptResponseDto>> GetAllUserQuizAttemptByQuizIdAsync(Guid quizId)
        {
            var quizAttempts = await _userQuizAttemptRepository.GetAllUserQuizAttemptByQuizIdAsync(quizId);

            if (!quizAttempts.Any())
                throw new ApiException.NotFoundException("No quiz attempts found.");


            var result = await Task.WhenAll(quizAttempts.Select(a => MapToDTO(a)));
            return result.ToList();
        }

        public async Task<List<UserQuizAttemptResponseDto>> GetUserQuizAtempByQuizId(Guid quizId)
        {
            var quizAttempts = await _userQuizAttemptRepository.GetAllUserQuizAttemptByQuizIdAsync(quizId);

            if (quizAttempts == null || !quizAttempts.Any())
            {
                Console.WriteLine($"[INFO] Quiz {quizId} has 0 attempts, returning empty list.");
                return new List<UserQuizAttemptResponseDto>();
            }

            var result = new List<UserQuizAttemptResponseDto>();
            foreach (var attempt in quizAttempts)
            {
             
                var dto = await MapToDTO(attempt);
                if (dto != null)
                {
                    result.Add(dto);
                }
            }

            Console.WriteLine($"[INFO] Quiz {quizId} attempts mapped: {result.Count}");
            return result;
        }

        public async Task<PagedResult<UserQuizAttemptResponseDto>> GetAllUserQuizAttempByUserIdAsync(int pageNumber, int pageSize, Guid userId)
        {
            Console.WriteLine($"[INFO] GetAllUserQuizAttempByUserIdAsync called for UserId: {userId}, Page: {pageNumber}, PageSize: {pageSize}");

            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var existQuiz = await _quizRepository.GetAllQuizByUserId(userId);
            Console.WriteLine($"[INFO] Found {existQuiz.Count} quizzes for user {userId}");

            if (!existQuiz.Any())
            {
                Console.WriteLine($"[WARN] User {userId} has not done any quiz yet");
                throw new ApiException.NotFoundException("User has not done any quiz yet.");
            }

            var allAttempts = new List<UserQuizAttemptResponseDto>();

            foreach (var quiz in existQuiz)
            {
                var attempts = await GetUserQuizAtempByQuizId(quiz.Id);

                if (attempts.Any())
                {
                    Console.WriteLine($"[INFO] Adding {attempts.Count} attempts for quiz {quiz.Id}");
                    allAttempts.AddRange(attempts);
                }
                else
                {
                    Console.WriteLine($"[INFO] Quiz {quiz.Id} skipped because it has 0 attempts");
                }
            }

            if (!allAttempts.Any())
            {
                Console.WriteLine($"[WARN] No attempts found for user {userId}");
                throw new ApiException.NotFoundException("No attempts found for this user.");
            }

            int totalCount = allAttempts.Count;
            Console.WriteLine($"[INFO] Total attempts collected: {totalCount}");

            var pagedAttempts = allAttempts
                .OrderByDescending(a => a.Point)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            Console.WriteLine($"[INFO] Returning {pagedAttempts.Count} attempts for page {pageNumber}");

            return new PagedResult<UserQuizAttemptResponseDto>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = pagedAttempts
            };
        }



        public async Task<UserQuizAttemptResponseDto> SubmitAnswerAsync(SubmitAnswerRequest newSubmit)
        {

            var validatorResult = await _submitAnswerRequestValidator.ValidateAsync(newSubmit);
            if (!validatorResult.IsValid)
                throw new ApiException.ValidationException(validatorResult.Errors);

            if (newSubmit.AnswerList == null || !newSubmit.AnswerList.Any())
                throw new ApiException.BadRequestException("No answers provided.");


            var newAttempt = new UserQuizAttemptRequestDto
            {
                QuizId = newSubmit.QuizId,
                StartAt = newSubmit.StartTime ?? DateTime.UtcNow,
                FinishAt = newSubmit.FinishTime ?? DateTime.UtcNow,
                Point = 0,
                Accuracy = 0
            };

            var userAttempt = await CreateAsync(newAttempt);


            foreach (var answer in newSubmit.AnswerList)
            {
                var newAnswer = new AnswerRequestDto
                {
                    QuestionChoiceId = answer.QuestionChoiceId,
                    QuestionId = answer.QuestionId,
                    UserQuizAttemptId = userAttempt.Id
                };

                await _answerService.CreateNewAnswerAsync(newAnswer);
            }


            var newUserAttempt = await GetByIdAsync(userAttempt.Id);


            var quiz = await _quizRepository.GetQuizByIdAsync(newSubmit.QuizId);
            var questions = quiz.Questions.ToList();

            double totalPoints = quiz.TotalPoints;
            double earnedPoints = 0;
            int correctCount = 0;


            foreach (var ans in newUserAttempt.Answers)
            {
                var choice = await _questionChoiceService.GetQuestionChoiceByIdAsync(ans.QuestionChoiceId);

                if (choice != null && choice.IsCorrect)
                {
                    correctCount++;

                    var question = questions.FirstOrDefault(q => q.Id == ans.QuestionId);
                    if (question != null)
                        earnedPoints += question.Point;
                }
            }


            int totalQuestion = questions.Count;
            double accuracy = totalQuestion > 0 ? Math.Round((double)correctCount / totalQuestion * 100, 2) : 0;
            if (earnedPoints > totalPoints)
                earnedPoints = totalPoints;


            var updateRequest = new UserQuizAttemptRequestDto
            {
                QuizId = newUserAttempt.QuizId,
                StartAt = newUserAttempt.StartAt,
                FinishAt = newUserAttempt.FinishAt,
                Accuracy = Math.Round(accuracy, 2),
                Point = Math.Round(earnedPoints, 2)
            };

            var response = await UpdateByIdAsync(newUserAttempt.Id, updateRequest);
            return response;
        }


        public async Task<UserQuizAttemptResponseDto> CreateAsync(UserQuizAttemptRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Quiz attempt request cannot be null.");

            var validationResult = await _userQuizAttemptValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newAttempt = MapToEntity(request);
            await _userQuizAttemptRepository.AddAsync(newAttempt);

            return await MapToDTO(newAttempt);
        }


        public async Task<UserQuizAttemptResponseDto> UpdateByIdAsync(Guid id, UserQuizAttemptRequestDto request)
        {
            var existing = await _userQuizAttemptRepository.GetUserQuizAttemptsByIdAsync(id);
            if (existing == null)
                throw new ApiException.NotFoundException($"Quiz attempt with ID {id} not found.");

            if (request == null)
                throw new ApiException.BadRequestException("Request data cannot be null.");

            var validationResult = await _userQuizAttemptValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existing.QuizId = request.QuizId;
            existing.StartAt = request.StartAt;
            existing.FinishAt = request.FinishAt;
            existing.Point = request.Point;
            existing.Accuracy = request.Accuracy;

            await _userQuizAttemptRepository.UpdateAsync(existing);

            return await MapToDTO(existing);
        }


        public async Task<UserQuizAttemptResponseDto> DeleteByIdAsync(Guid id)
        {
            var existing = await _userQuizAttemptRepository.GetUserQuizAttemptsByIdAsync(id);
            if (existing == null)
                throw new ApiException.NotFoundException($"Quiz attempt with ID {id} not found.");

            await _userQuizAttemptRepository.DeleteAsync(existing);

            return await MapToDTO(existing);
        }

        public async Task<UserQuizAttemptResponseDto> MapToDTO(UserQuizAttempt attempt)
        {
            if (attempt == null) return null;

            var existQuiz = await _quizRepository.GetQuizByIdAsync(attempt.QuizId);
            if (existQuiz == null)
            {
                Console.WriteLine($"[WARN] Quiz {attempt.QuizId} not found for attempt {attempt.Id}");
                return null;
            }

            bool passed = false;

            if (attempt.Point >= existQuiz.PassScore)
            {
                passed = true;
            }

                return new UserQuizAttemptResponseDto
                {
                    Id = attempt.Id,
                    QuizId = attempt.QuizId,
                    StartAt = attempt.StartAt,
                    FinishAt = attempt.FinishAt,
                    TotalPoints = existQuiz.TotalPoints,
                    PassScore = existQuiz.PassScore,
                    QuizName = existQuiz.Name,
                    TotalQuestion = existQuiz.TotalQuestion,
                    IsPassed = passed,
                    Point = attempt.Point,
                    Accuracy = attempt.Accuracy,
                    Answers = attempt.Answers != null
                        ? attempt.Answers.Select(a => _answerService.MapToDTO(a)).ToList()
                        : new List<AnswerResponseDto>()
                };
        }

        protected UserQuizAttempt MapToEntity(UserQuizAttemptRequestDto request)
        {
            return new UserQuizAttempt
            {
                QuizId = request.QuizId,
                StartAt = request.StartAt,
                FinishAt = request.FinishAt,
                Point = request.Point,
                Accuracy = request.Accuracy
            };
        }
    }
}
