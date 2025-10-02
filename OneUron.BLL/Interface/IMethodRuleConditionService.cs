using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IMethodRuleConditionService
    {
        public Task<ApiResponse<List<MethodRuleConditionResponseDto>>> GetAllAsync();

        public Task<ApiResponse<MethodRuleConditionResponseDto>> GetByIdAsync(Guid id);

        public  Task<ApiResponse<MethodRuleConditionResponseDto>> CreateNewMethodRuleConditionAsync(MethodRuleConditionRequestDto request);

        public  Task<ApiResponse<MethodRuleConditionResponseDto>> UpdateMethodRuleConditionByIdAsync(Guid id, MethodRuleConditionRequestDto newMethodRuleCondtion);

        public  Task<ApiResponse<MethodRuleConditionResponseDto>> DeleteMethodRuleConditionByIdAsync(Guid id);

        public MethodRuleConditionResponseDto MapToDTO(MethodRuleCondition methodRuleCondition);

        public Task<MethodRuleConditionResponseDto> GetMethodRuleConditionByChoiceId(Guid choiceId);

    }
}
