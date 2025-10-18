using FluentValidation;
using OneUron.BLL.DTOs.AnswerDTOs;
using OneUron.BLL.DTOs.QuestionDTOs;
using OneUron.BLL.DTOs.UserAnswerDTOs;
using OneUron.BLL.DTOs.UserQuizAttemptDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
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

            return attempts.Select(MapToDTO).ToList();
        }


        public async Task<UserQuizAttemptResponseDto> GetByIdAsync(Guid id)
        {
            var attempt = await _userQuizAttemptRepository.GetUserQuizAttemptsByIdAsync(id);
            if (attempt == null)
                throw new ApiException.NotFoundException($"Quiz attempt with ID {id} not found.");

            return MapToDTO(attempt);
        }

        public async Task<List<UserQuizAttemptResponseDto>> GetAllUserQuizAttemptByQuizIdAsync(Guid quizId)
        {
            var quizAttempts = await _userQuizAttemptRepository.GetAllUserQuizAttemptByQuizIdAsync(quizId);

            if (!quizAttempts.Any())
                throw new ApiException.NotFoundException("No quiz attempts found.");

            var result = quizAttempts.Select(MapToDTO).ToList();
            return result;
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
            double accuracy = totalQuestion > 0? Math.Round((double)correctCount / totalQuestion * 100, 2) : 0;
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

            return MapToDTO(newAttempt);
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

            return MapToDTO(existing);
        }


        public async Task<UserQuizAttemptResponseDto> DeleteByIdAsync(Guid id)
        {
            var existing = await _userQuizAttemptRepository.GetUserQuizAttemptsByIdAsync(id);
            if (existing == null)
                throw new ApiException.NotFoundException($"Quiz attempt with ID {id} not found.");

            await _userQuizAttemptRepository.DeleteAsync(existing);

            return MapToDTO(existing);
        }

        public UserQuizAttemptResponseDto MapToDTO(UserQuizAttempt attempt)
        {
            if (attempt == null) return null;

            return new UserQuizAttemptResponseDto
            {
                Id = attempt.Id,
                QuizId = attempt.QuizId,
                StartAt = attempt.StartAt,
                FinishAt = attempt.FinishAt,
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
