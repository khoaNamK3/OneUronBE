
using FluentValidation;
using OneUron.BLL.DTOs.AcknowledgeDTOs;
using OneUron.BLL.DTOs.ChoiceDTOs;
using OneUron.BLL.DTOs.EnRollDTOs;
using OneUron.BLL.DTOs.EvaluationDTOs;
using OneUron.BLL.DTOs.EvaluationQuestionDTOs;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.DTOs.UserAnswerDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.EvaluationQuestionRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class EvaluationQuestionService : IEvaluationQuestionService
    {
        private readonly IEvaluationQuestionRepository _evaluationQuestionRepository;
        private readonly IChoiceService _choiceService;
        private readonly IUserAnswerService _userAnswerService;
        private readonly IMethodRuleConditionService _methodRuleConditionService;
        private readonly IValidator<EvaluationQuestionRequestDto> _evaluationQuestionValidator;

        public EvaluationQuestionService(
            IEvaluationQuestionRepository evaluationQuestionRepository,
            IChoiceService choiceService,
            IUserAnswerService userAnswerService,
            IMethodRuleConditionService methodRuleConditionService,
            IValidator<EvaluationQuestionRequestDto> evaluationQuestionValidator)
        {
            _evaluationQuestionRepository = evaluationQuestionRepository;
            _choiceService = choiceService;
            _userAnswerService = userAnswerService;
            _methodRuleConditionService = methodRuleConditionService;
            _evaluationQuestionValidator = evaluationQuestionValidator;
        }

       
        public async Task<List<EvaluationQuestionResponseDto>> GetAllAsync()
        {
            var evaluationQuestions = await _evaluationQuestionRepository.GetAllAsync();

            if (evaluationQuestions == null || !evaluationQuestions.Any())
                throw new ApiException.NotFoundException("No EvaluationQuestions found.");

            return evaluationQuestions.Select(MapToDTO).ToList();
        }

        
        public async Task<EvaluationQuestionResponseDto> GetByIdAsync(Guid id)
        {
            var existEvaluationQuestion = await _evaluationQuestionRepository.GetEvaluationQuestionByIdAsync(id);

            if (existEvaluationQuestion == null)
                throw new ApiException.NotFoundException($"EvaluationQuestion with ID {id} not found.");

            return MapToDTO(existEvaluationQuestion);
        }

       
        public async Task<EvaluationQuestionResponseDto> CreateNewEvaluationQuestionAsync(EvaluationQuestionRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("EvaluationQuestion request cannot be null.");

            var validationResult = await _evaluationQuestionValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newEvaluationQuestion = MapToEntity(request);
            await _evaluationQuestionRepository.AddAsync(newEvaluationQuestion);

            return MapToDTO(newEvaluationQuestion);
        }

        
        public async Task<EvaluationQuestionResponseDto> UpdateEvaluationQuestionByIdAsync(Guid id, EvaluationQuestionRequestDto newEvaluation)
        {
            var existEvaluationQuestion = await _evaluationQuestionRepository.GetEvaluationQuestionByIdAsync(id);
            if (existEvaluationQuestion == null)
                throw new ApiException.NotFoundException($"EvaluationQuestion with ID {id} not found.");

            if (newEvaluation == null)
                throw new ApiException.BadRequestException("New EvaluationQuestion data cannot be null.");

            var validationResult = await _evaluationQuestionValidator.ValidateAsync(newEvaluation);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existEvaluationQuestion.Title = newEvaluation.Title;
            existEvaluationQuestion.Description = newEvaluation.Description;
            existEvaluationQuestion.Type = newEvaluation.Type;
            existEvaluationQuestion.EvaluationId = newEvaluation.EvaluationId;

            await _evaluationQuestionRepository.UpdateAsync(existEvaluationQuestion);

            return MapToDTO(existEvaluationQuestion);
        }

        
        public async Task<EvaluationQuestionResponseDto> DeleteEvaluationQuestionByIdAsync(Guid id)
        {
            var existEvaluationQuestion = await _evaluationQuestionRepository.GetEvaluationQuestionByIdAsync(id);
            if (existEvaluationQuestion == null)
                throw new ApiException.NotFoundException($"EvaluationQuestion with ID {id} not found.");

            await _evaluationQuestionRepository.DeleteAsync(existEvaluationQuestion);

            return MapToDTO(existEvaluationQuestion);
        }

       
        protected EvaluationQuestion MapToEntity(EvaluationQuestionRequestDto request)
        {
            return new EvaluationQuestion
            {
                Title = request.Title,
                Description = request.Description,
                Type = request.Type,
                EvaluationId = request.EvaluationId
            };
        }

        public EvaluationQuestionResponseDto MapToDTO(EvaluationQuestion evaluationQuestion)
        {
            if (evaluationQuestion == null) return null;

            return new EvaluationQuestionResponseDto
            {
                Id = evaluationQuestion.Id,
                Title = evaluationQuestion.Title,
                Description = evaluationQuestion.Description,
                Type = evaluationQuestion.Type,
                EvaluationId = evaluationQuestion.EvaluationId,

                Choices = evaluationQuestion.Choices?
                    .Select(c => _choiceService.MapToDTO(c))
                    .ToList() ?? new List<ChoiceResponseDto>(),

               
                // UserAnswers = evaluationQuestion.UserAnswers?
                //     .Select(u => _userAnswerService.MaptoDTO(u))
                //     .ToList() ?? new List<UserAnswerResponseDto>(),

                // MethodRuleConditions = evaluationQuestion.MethodRuleConditions?
                //     .Select(mrc => _methodRuleConditionService.MapToDTO(mrc))
                //     .ToList() ?? new List<MethodRuleConditionResponseDto>()
            };
        }
    }
}
