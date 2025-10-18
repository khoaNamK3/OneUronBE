using FluentValidation;
using OneUron.BLL.DTOs.EvaluationDTOs;
using OneUron.BLL.DTOs.TechniqueDTOs;
using OneUron.BLL.DTOs.UserAnswerDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.UserAnswerRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly IUserAnswerRepository _userAnswerRepository;
        private readonly IValidator<UserAnswerRequestDto> _userAnswerValidator;

        public UserAnswerService(
            IUserAnswerRepository userAnswerRepository,
            IValidator<UserAnswerRequestDto> userAnswerValidator)
        {
            _userAnswerRepository = userAnswerRepository;
            _userAnswerValidator = userAnswerValidator;
        }


        public async Task<List<UserAnswerResponseDto>> GetAllAsync()
        {
            var userAnswers = await _userAnswerRepository.GetAllAsync();

            if (userAnswers == null || !userAnswers.Any())
                throw new ApiException.NotFoundException("No user answers found.");

            return userAnswers.Select(MapToDTO).ToList();
        }

 
        public async Task<List<UserAnswerResponseDto>> GetByListAsync(Guid userId, Guid evaluationQuestionId)
        {
            var userAnswers = await _userAnswerRepository.GetByListUserAnswerAsync(userId, evaluationQuestionId);

            if (userAnswers == null || !userAnswers.Any())
                throw new ApiException.NotFoundException("No user answers found for given user and question.");

            return userAnswers.Select(MapToDTO).ToList();
        }

 
        public async Task<UserAnswerResponseDto> CreateAsync(UserAnswerRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("UserAnswer request cannot be null.");

            var validationResult = await _userAnswerValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newUserAnswer = MapToEntity(request);
            await _userAnswerRepository.AddAsync(newUserAnswer);

            return MapToDTO(newUserAnswer);
        }


        public async Task<UserAnswerResponseDto> UpdateByIdAsync(Guid id, UserAnswerUpdateRequestDto request)
        {
            var existing = await _userAnswerRepository.GetByIdAsync(id);
            if (existing == null)
                throw new ApiException.NotFoundException($"UserAnswer with ID {id} not found.");

            if (request == null)
                throw new ApiException.BadRequestException("Request data cannot be null.");

            existing.ChoiceId = request.ChoiceId;

            await _userAnswerRepository.UpdateAsync(existing);

            return MapToDTO(existing);
        }


        public async Task<UserAnswerResponseDto> DeleteByIdAsync(Guid id)
        {
            var existing = await _userAnswerRepository.GetByIdAsync(id);
            if (existing == null)
                throw new ApiException.NotFoundException($"UserAnswer with ID {id} not found.");

            await _userAnswerRepository.DeleteAsync(existing);

            return MapToDTO(existing);
        }

 
        public async Task<List<UserAnswerResponseDto>> SubmitAnswersAsync(List<EvaluationSubmitRequest> evaluations)
        {
            if (evaluations == null || !evaluations.Any())
                throw new ApiException.BadRequestException("Evaluations cannot be empty.");

            var results = new List<UserAnswerResponseDto>();

            foreach (var eval in evaluations)
            {
                if (eval.Questions == null || !eval.Questions.Any())
                    continue;

                var oldAnswers = await _userAnswerRepository.GetUserAnswerByEvaluationIdAsync(eval.UserId, eval.EvaluationId);
                if (oldAnswers != null && oldAnswers.Any())
                    await _userAnswerRepository.DeleteRangeAsync(oldAnswers);

                foreach (var question in eval.Questions)
                {
                    if (eval.UserId == Guid.Empty || question.EvaluationQuestionId == Guid.Empty || question.ChoiceId == Guid.Empty)
                        continue;

                    var entity = new UserAnswer
                    {
                        Id = Guid.NewGuid(),
                        UserId = eval.UserId,
                        EvaluationQuestionId = question.EvaluationQuestionId,
                        ChoiceId = question.ChoiceId
                    };

                    await _userAnswerRepository.AddAsync(entity);
                    results.Add(MapToDTO(entity));
                }
            }

            return results;
        }


        public async Task<List<UserAnswerResponseDto>> GetAllByUserIdAsync(Guid userId)
        {
            var userAnswers = await _userAnswerRepository.GetAllUserAnswerByUserIdAsync(userId);

            if (userAnswers == null || !userAnswers.Any())
                throw new ApiException.NotFoundException($"No user answers found for UserId {userId}.");

            return userAnswers.Select(MapToDTO).ToList();
        }

      
        protected UserAnswer MapToEntity(UserAnswerRequestDto request)
        {
            return new UserAnswer
            {
                UserId = request.UserId,
                ChoiceId = request.ChoiceId,
                EvaluationQuestionId = request.EvaluationQuestionId
            };
        }

        public UserAnswerResponseDto MapToDTO(UserAnswer userAnswer)
        {
            if (userAnswer == null) return null;

            return new UserAnswerResponseDto
            {
                Id = userAnswer.Id,
                UserId = userAnswer.UserId,
                ChoiceId = userAnswer.ChoiceId,
                EvaluationQuestionId = userAnswer.EvaluationQuestionId
            };
        }
    }
}
