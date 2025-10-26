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
        Task<List<MethodRuleConditionResponseDto>> GetAllAsync();
        Task<MethodRuleConditionResponseDto> GetByIdAsync(Guid id);
        Task<MethodRuleConditionResponseDto> CreateNewMethodRuleConditionAsync(MethodRuleConditionRequestDto request);
        Task<MethodRuleConditionResponseDto> UpdateMethodRuleConditionByIdAsync(Guid id, MethodRuleConditionRequestDto newMethodRuleCondition);
        Task<MethodRuleConditionResponseDto> DeleteMethodRuleConditionByIdAsync(Guid id);
        Task<List<MethodRuleConditionResponseDto>> GetMethodRuleConditionByChoiceId(Guid choiceId);

        public MethodRuleConditionResponseDto MapToDTO(MethodRuleCondition methodRuleCondition);


    }
}
