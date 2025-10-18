using OneUron.BLL.DTOs.StudyMethodDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IStudyMethodService
    {
        Task<List<StudyMethodResponseDto>> GetAllAsync();
        Task<StudyMethodResponseDto> GetByIdAsync(Guid id);
        Task<StudyMethodResponseDto> CreateAsync(StudyMethodRequestDto request);
        Task<StudyMethodResponseDto> UpdateByIdAsync(Guid id, StudyMethodRequestDto request);
        Task<StudyMethodResponseDto> DeleteByIdAsync(Guid id);
        StudyMethodResponseDto MapToDTO(StudyMethod studyMethod);
        Task<StudyMethodResponseDto> GetStudyMethodByUserIdAsync(Guid userId);
    }
}
