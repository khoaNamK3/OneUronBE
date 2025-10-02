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
        public Task<ApiResponse<List<MethodRuleResponseDto>>> GetAllAsync();

        public Task<ApiResponse<MethodRuleResponseDto>> GetByIdAsync(Guid id);

        public  Task<ApiResponse<MethodRuleResponseDto>> CreateNewMethodRuleAsync(MethodRuleRequestDto request);

        public  Task<ApiResponse<MethodRuleResponseDto>> UpdateMethodRuleByIdAsync(Guid id, MethodRuleRequestDto newMethodRule);

        public  Task<ApiResponse<MethodRuleResponseDto>> DeleteMethodRuleByIdAsync(Guid id);

        public MethodRuleResponseDto MapToDTO(MethodRule methodRule);
    }
}
