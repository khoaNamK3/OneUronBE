using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.DTOs.SubjectDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface ISubjectService
    {
        Task<List<SubjectResponseDto>> GetAllAsync();
        Task<SubjectResponseDto> GetByIdAsync(Guid id);
        Task<SubjectResponseDto> CreateAsync(SubjectRequestDto request);
        Task<SubjectResponseDto> UpdateByIdAsync(Guid id, SubjectRequestDto request);
        Task<SubjectResponseDto> DeleteByIdAsync(Guid id);
        SubjectResponseDto MapToDTO(Subject subject);
        Task<List<SubjectResponseDto>> GetAllSubjectbyScheduleIdAsync(Guid scheduleId);

        public  Task<List<SubjectResponseDto>> GetSubjectByProcessIdAsync(Guid processId);
    }
}
