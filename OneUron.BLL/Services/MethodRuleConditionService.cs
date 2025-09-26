using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
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
        public MethodRuleConditionService(IMethodRuleConditionRepository methodRuleConditionRepository, IMethodRuleService methodRuleService)
        {
            _methodRuleConditionRepository = methodRuleConditionRepository;
            _methodRuleService = methodRuleService;
        }

        public async Task<ApiResponse<List<MethodRuleConditionResponseDto>>> GetAllAsync()
        {
            try
            {
                var methodRuleCondtions = await _methodRuleConditionRepository.GetAllAsync();

                if (!methodRuleCondtions.Any())
                {
                    return ApiResponse<List<MethodRuleConditionResponseDto>>.FailResponse("Get All MethodRuleCondition Fail", "MethodRuleCondtion Are empty");
                }

                var result = methodRuleCondtions.Select(MapToDTO).ToList();

                return ApiResponse<List<MethodRuleConditionResponseDto>>.SuccessResponse(result, "Get All MethodRuleContion Successfully");
            }
            catch (Exception ex)
            {

                return ApiResponse<List<MethodRuleConditionResponseDto>>.FailResponse("Get All MethodRuleCondition Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodRuleConditionResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var existMethodRuleCondition = await _methodRuleConditionRepository.GetByIdAsync(id);

                if (existMethodRuleCondition == null)
                {
                    return ApiResponse<MethodRuleConditionResponseDto>.FailResponse("Get MethodRuleContion By Id Fail", "MethodRuleContion is not exist");
                }

                var result = MapToDTO(existMethodRuleCondition);

                return ApiResponse<MethodRuleConditionResponseDto>.SuccessResponse(result, "Get All MethodRuleCondtion Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodRuleConditionResponseDto>.FailResponse("Get MethodRuleContion By Id Fail", ex.Message);
            }

        }

        public async Task<ApiResponse<MethodRuleConditionResponseDto>> CreateNewMethodRuleConditionAsync(MethodRuleConditionRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<MethodRuleConditionResponseDto>.FailResponse("Create New MethodRuleContion Fail", "New MethodRuleCondtion are Empty");
                }

                var newMethodRuleCondition = MapToEntity(request);

                await _methodRuleConditionRepository.AddAsync(newMethodRuleCondition);

                var result = MapToDTO(newMethodRuleCondition);

                return ApiResponse<MethodRuleConditionResponseDto>.SuccessResponse(result, "Create New MethodRuleCondition Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodRuleConditionResponseDto>.FailResponse("Create New MethodRuleContion Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodRuleConditionResponseDto>> UpdateMethodRuleConditionByIdAsync(Guid id, MethodRuleConditionRequestDto newMethodRuleCondtion)
        {
            try
            {
                var existMethodRuleCondition = await _methodRuleConditionRepository.GetByIdAsync(id);

                if (existMethodRuleCondition == null)
                {
                    return ApiResponse<MethodRuleConditionResponseDto>.FailResponse("Update MethodRuleContion By Id Fail", "MethodRuleContion is not exist");
                }

                if (newMethodRuleCondtion == null)
                {
                    return ApiResponse<MethodRuleConditionResponseDto>.FailResponse("Update MethodRuleContion By Id Fail", "New MethodRuleContion is Empty");
                }
                existMethodRuleCondition.Weight = newMethodRuleCondtion.Weight;
                existMethodRuleCondition.Effectiveness = newMethodRuleCondtion.Effectiveness;
                existMethodRuleCondition.ChoiceId = newMethodRuleCondtion.ChoiceId;
                existMethodRuleCondition.EvaluationId = newMethodRuleCondtion.EvaluationId;
                existMethodRuleCondition.EvaluationQuestionId = newMethodRuleCondtion.EvaluationQuestionId;

                await _methodRuleConditionRepository.UpdateAsync(existMethodRuleCondition);

                var result = MapToDTO(existMethodRuleCondition);

                return ApiResponse<MethodRuleConditionResponseDto>.SuccessResponse(result, "Update MethodRuleCondtion By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodRuleConditionResponseDto>.FailResponse("Update MethodRuleContion By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodRuleConditionResponseDto>> DeleteMethodRuleConditionByIdAsync(Guid id)
        {
            try
            {
                var existMethodRuleCondition = await _methodRuleConditionRepository.GetByIdAsync(id);

                if (existMethodRuleCondition == null)
                {
                    return ApiResponse<MethodRuleConditionResponseDto>.FailResponse("Delete MethodRuleContion By Id Fail", "MethodRuleContion is not exist");
                }
                var result = MapToDTO(existMethodRuleCondition);

                await _methodRuleConditionRepository.DeleteAsync(existMethodRuleCondition);

                return ApiResponse<MethodRuleConditionResponseDto>.SuccessResponse(result, "Delete MethodRuleCondtion Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodRuleConditionResponseDto>.FailResponse("Delete MethodRuleContion By Id Fail", ex.Message);
            }
        }

        protected MethodRuleCondition MapToEntity(MethodRuleConditionRequestDto request)
        {
            return new MethodRuleCondition
            {
                Effectiveness = request.Effectiveness,
                Weight = request.Weight,
                ChoiceId = request.ChoiceId,
                EvaluationId = request.EvaluationId,
                EvaluationQuestionId = request.EvaluationQuestionId,
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
                    .Select(mrc => _methodRuleService.MapToDTO(mrc))
                    .ToList() ?? new List<MethodRuleResponseDto>()
            };
        }

    }
}
