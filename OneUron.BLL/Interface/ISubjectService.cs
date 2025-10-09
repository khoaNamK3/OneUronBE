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
        public Task<ApiResponse<List<SubjectResponseDto>>> GetAllAsync();

        public Task<ApiResponse<SubjectResponseDto>> GetByIdAsync(Guid id);

        public  Task<ApiResponse<SubjectResponseDto>> CreateNewSubjectAsync(SubjectRequestDto request);

        public  Task<ApiResponse<SubjectResponseDto>> UpdateSubjectByIdAsync(Guid id, SubjectRequestDto newSubject);

        public  Task<ApiResponse<SubjectResponseDto>> DeleteSubjectByIdAsync(Guid id);

        public SubjectResponseDto MapToDTO(Subject subject);


    }
}
