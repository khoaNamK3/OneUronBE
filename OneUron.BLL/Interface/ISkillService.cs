using OneUron.BLL.DTOs.SkillDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface ISkillService
    {
        public Task<ApiResponse<List<SkillResponseDto>>> GetAllAsync();

        public Task<ApiResponse<SkillResponseDto>> GetByIdAsync(Guid id);

        public  Task<ApiResponse<SkillResponseDto>> CreateNewSkillAsync(SkillRequestDto request);

        public  Task<ApiResponse<SkillResponseDto>> UpdateSkillByIdAsync(Guid id, SkillRequestDto newSkill);

        public  Task<ApiResponse<SkillResponseDto>> DeleteSkillByIdAsync(Guid id);

        public SkillResponseDto MapToDTO(Skill skill);
    }
}
