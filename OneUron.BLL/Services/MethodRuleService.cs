using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.MethodRulesRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class MethodRuleService : IMethodRuleService
    {
        private readonly IMethodRuleRepository _methodRuleRepository;

        public MethodRuleService(IMethodRuleRepository methodRuleRepository)
        {
            _methodRuleRepository = methodRuleRepository;
        }

        public async Task<ApiResponse<List<MethodRuleResponseDto>>> GetAllAsync()
        {
            try
            {
                var methodRuleCondtions = await _methodRuleRepository.GetAllAsync();

                if (!methodRuleCondtions.Any())
                {
                    return ApiResponse<List<MethodRuleResponseDto>>.FailResponse("Get All MethodRule Fail", "MethodRules Are Empty");
                }

                var result = methodRuleCondtions.Select(MapToDTO).ToList();

                return ApiResponse<List<MethodRuleResponseDto>>.SuccessResponse(result, "Get All MethodRule Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<MethodRuleResponseDto>>.FailResponse("Get All MethodRule Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodRuleResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var existMethodRule = await _methodRuleRepository.GetByIdAsync(id);

                if (existMethodRule == null)
                {
                    return ApiResponse<MethodRuleResponseDto>.FailResponse("Get MethodRule By Id Fail", "MethodRule is not exist");
                }
                var result = MapToDTO(existMethodRule);

                return ApiResponse<MethodRuleResponseDto>.SuccessResponse(result, "Get MethodRule By Id successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodRuleResponseDto>.FailResponse("Get MethodRule By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodRuleResponseDto>> CreateNewMethodRuleAsync(MethodRuleRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<MethodRuleResponseDto>.FailResponse("Create new MethodRule Fail", "New MethodRule Is null");
                }

                var newMethodRule = MaptoEntity(request);

                await _methodRuleRepository.AddAsync(newMethodRule);

                var result = MapToDTO(newMethodRule);

                return ApiResponse<MethodRuleResponseDto>.SuccessResponse(result, "Create New MethodRule Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodRuleResponseDto>.FailResponse("Create new MethodRule Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodRuleResponseDto>> UpdateMethodRuleByIdAsync(Guid id, MethodRuleRequestDto newMethodRule)
        {
            try
            {
                var existMethodRule = await _methodRuleRepository.GetByIdAsync(id);

                if (existMethodRule == null)
                {
                    return ApiResponse<MethodRuleResponseDto>.FailResponse("Update new MethodRule Fail", "MethodRule are not exist");
                }

                if (newMethodRule == null)
                {
                    return ApiResponse<MethodRuleResponseDto>.FailResponse("Update new MethodRule Fail", "New MethodRule Is null");
                }

                existMethodRule.MethodId = newMethodRule.MethodId;
                existMethodRule.MethodRuleConditionId = newMethodRule.MethodRuleConditionId;

                await _methodRuleRepository.UpdateAsync(existMethodRule);

                var result = MapToDTO(existMethodRule);

                return ApiResponse<MethodRuleResponseDto>.SuccessResponse(result, "Update MethodRule By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodRuleResponseDto>.FailResponse("Update new MethodRule Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodRuleResponseDto>> DeleteMethodRuleByIdAsync(Guid id)
        {
            try
            {
                var existMethodRule = await _methodRuleRepository.GetByIdAsync(id);

                if (existMethodRule == null)
                {
                    return ApiResponse<MethodRuleResponseDto>.FailResponse("Delete new MethodRule Fail", "MethodRule are not exist");
                }

                var result = MapToDTO(existMethodRule);

                await _methodRuleRepository.DeleteAsync(existMethodRule);

                return ApiResponse<MethodRuleResponseDto>.SuccessResponse(result, "Delete MethodRule By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodRuleResponseDto>.FailResponse("Delete new MethodRule Fail", ex.Message);
            }
        }

        protected MethodRule MaptoEntity(MethodRuleRequestDto request)
        {
            return new MethodRule
            {
                MethodId = request.MethodId,
                MethodRuleConditionId = request.MethodRuleConditionId,
            };
        }

        public MethodRuleResponseDto MapToDTO(MethodRule methodRule)
        {
            return new MethodRuleResponseDto
            {
                Id = methodRule.Id,
                MethodId = methodRule.MethodId,

                MethodRuleConditionId = methodRule.MethodRuleConditionId,
            };
        }
    }
}
