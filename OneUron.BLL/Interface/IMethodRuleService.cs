using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IMethodRuleService
    {
        Task<List<MethodRuleResponseDto>> GetAllAsync();
        Task<MethodRuleResponseDto> GetByIdAsync(Guid id);
        Task<MethodRuleResponseDto> CreateNewMethodRuleAsync(MethodRuleRequestDto request);
        Task<MethodRuleResponseDto> UpdateMethodRuleByIdAsync(Guid id, MethodRuleRequestDto newMethodRule);
        Task<MethodRuleResponseDto> DeleteMethodRuleByIdAsync(Guid id);

        public MethodRuleResponseDto MapToDTO(MethodRule methodRule);
    }
}
