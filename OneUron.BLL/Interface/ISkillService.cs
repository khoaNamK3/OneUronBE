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
        Task<List<SkillResponseDto>> GetAllAsync();
        Task<SkillResponseDto> GetByIdAsync(Guid id);
        Task<SkillResponseDto> CreateNewSkillAsync(SkillRequestDto request);
        Task<SkillResponseDto> UpdateSkillByIdAsync(Guid id, SkillRequestDto request);
        Task<SkillResponseDto> DeleteSkillByIdAsync(Guid id);

        public SkillResponseDto MapToDTO(Skill skill);
    }
}
