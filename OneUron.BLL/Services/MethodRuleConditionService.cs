using FluentValidation;
using OneUron.BLL.DTOs.MethodProDTOs;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.EvaluationQuestionRepo;
using OneUron.DAL.Repository.EvaluationRepo;
using OneUron.DAL.Repository.MethodRuleConditionRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class MethodRuleConditionService : IMethodRuleConditionService
    {
        private readonly IMethodRuleConditionRepository _methodRuleConditionRepository;
        private readonly IMethodRuleService _methodRuleService;
        private readonly IEvaluationQuestionRepository _evaluationQuestionRepository;
        private readonly IEvaluationRepository _evaluationRepository;
        private readonly IValidator<MethodRuleConditionRequestDto> _methodRuleConditionValidator;

        public MethodRuleConditionService(
            IMethodRuleConditionRepository methodRuleConditionRepository,
            IMethodRuleService methodRuleService,
            IValidator<MethodRuleConditionRequestDto> methodRuleConditionValidator,
            IEvaluationQuestionRepository evaluationQuestionRepository,
            IEvaluationRepository evaluationRepository
            )
        {
            _methodRuleConditionRepository = methodRuleConditionRepository;
            _methodRuleService = methodRuleService;
            _methodRuleConditionValidator = methodRuleConditionValidator;
            _evaluationQuestionRepository = evaluationQuestionRepository;
            _evaluationRepository = evaluationRepository;
        }

        
        public async Task<List<MethodRuleConditionResponseDto>> GetAllAsync()
        {
            var methodRuleConditions = await _methodRuleConditionRepository.GetAllAsync();

            if (methodRuleConditions == null || !methodRuleConditions.Any())
                throw new ApiException.NotFoundException("Điều kiện phương pháp học không tìm thấy.");

            return methodRuleConditions.Select(MapToDTO).ToList();
        }

        public async Task<MethodRuleConditionResponseDto> GetByIdAsync(Guid id)
        {
            var existMethodRuleCondition = await _methodRuleConditionRepository.GetByIdAsync(id);
            if (existMethodRuleCondition == null)
                throw new ApiException.NotFoundException($"Điều kiện phương pháp học của ID {id} không tìm thấy.");

            return MapToDTO(existMethodRuleCondition);
        }

       
        public async Task<MethodRuleConditionResponseDto> CreateNewMethodRuleConditionAsync(MethodRuleConditionRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Điều kiện phương pháp học mới không được để trống.");

            var existEvalutionQuestion = await _evaluationQuestionRepository.GetEvaluationQuestionByIdAsync(request.ChoiceId);
            if (existEvalutionQuestion == null)
                throw new ApiException.NotFoundException("Not found EvalutionQuestion");

            var existEvaltion = await _evaluationRepository.GetByIdAsync(existEvalutionQuestion.EvaluationId);
            if (existEvaltion == null)
                throw new ApiException.NotFoundException("Not Found Evalution");

            var validationResult = await _methodRuleConditionValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newMethodRuleCondition = MapToEntity(request, existEvaltion, existEvalutionQuestion);

            await _methodRuleConditionRepository.AddAsync(newMethodRuleCondition);

            return MapToDTO(newMethodRuleCondition);
        }

        
        public async Task<MethodRuleConditionResponseDto> UpdateMethodRuleConditionByIdAsync(Guid id, MethodRuleConditionRequestDto newMethodRuleCondition)
        {
            var existMethodRuleCondition = await _methodRuleConditionRepository.GetByIdAsync(id);
            if (existMethodRuleCondition == null)
                throw new ApiException.NotFoundException($"Điều kiện phương pháp học của ID {id} không tìm thấy.");

            if (newMethodRuleCondition == null)
                throw new ApiException.BadRequestException("Điều kiện phương Pháp học mới không được để trống ");

            var validationResult = await _methodRuleConditionValidator.ValidateAsync(newMethodRuleCondition);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existMethodRuleCondition.Weight = newMethodRuleCondition.Weight;
            existMethodRuleCondition.Effectiveness = newMethodRuleCondition.Effectiveness;
            existMethodRuleCondition.ChoiceId = newMethodRuleCondition.ChoiceId;
            //existMethodRuleCondition.EvaluationId = newMethodRuleCondition.EvaluationId;
            //existMethodRuleCondition.EvaluationQuestionId = newMethodRuleCondition.EvaluationQuestionId;

            await _methodRuleConditionRepository.UpdateAsync(existMethodRuleCondition);

            return MapToDTO(existMethodRuleCondition);
        }

        
        public async Task<MethodRuleConditionResponseDto> DeleteMethodRuleConditionByIdAsync(Guid id)
        {
            var existMethodRuleCondition = await _methodRuleConditionRepository.GetByIdAsync(id);
            if (existMethodRuleCondition == null)
                throw new ApiException.NotFoundException($"MethodRuleCondition with ID {id} not found.");

            await _methodRuleConditionRepository.DeleteAsync(existMethodRuleCondition);

            return MapToDTO(existMethodRuleCondition);
        }

        
        public async Task<List<MethodRuleConditionResponseDto>> GetMethodRuleConditionByChoiceId(Guid choiceId)
        {
            var methodRuleCondition = await _methodRuleConditionRepository.GetMethodRuleConditionByChoiceId(choiceId);
            if (!methodRuleCondition.Any())
                throw new ApiException.NotFoundException("No method rulecondition found");

            var result  = methodRuleCondition.Select(MapToDTO).ToList();
            return result;
        }



        
        protected MethodRuleCondition MapToEntity(MethodRuleConditionRequestDto request,Evaluation evaluation, EvaluationQuestion evaluationQuestion)
        {
            return new MethodRuleCondition
            {
                Effectiveness = request.Effectiveness,
                Weight = request.Weight,
                ChoiceId = request.ChoiceId,
                EvaluationQuestionId = evaluationQuestion.Id,
                EvaluationId = evaluation.Id
            };
        }

        public MethodRuleConditionResponseDto MapToDTO(MethodRuleCondition methodRuleCondition)
        {
            if (methodRuleCondition == null) return null;

            return new MethodRuleConditionResponseDto
            {
                Id = methodRuleCondition.Id,
                ChoiceId = methodRuleCondition.ChoiceId,
                Effectiveness = methodRuleCondition.Effectiveness,
                Weight = methodRuleCondition.Weight,
                EvaluationId = methodRuleCondition.EvaluationId,
                EvaluationQuestionId = methodRuleCondition.EvaluationQuestionId,

                MethodRules = methodRuleCondition.MethodRules?
                    .Select(mr => _methodRuleService.MapToDTO(mr))
                    .ToList() ?? new List<MethodRuleResponseDto>()
            };
        }
    }
}
