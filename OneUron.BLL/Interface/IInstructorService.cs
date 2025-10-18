using OneUron.BLL.DTOs.InstructorDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IInstructorService
    {
        Task<List<InstructorResponseDto>> GetAllAsync();
        Task<InstructorResponseDto> GetInstructorByIdAsync(Guid id);
        Task<InstructorResponseDto> CreateNewInstructorAsync(InstructorRequestDto request);
        Task<InstructorResponseDto> UpdateInstructorByIdAsync(Guid id, InstructorRequestDto newInstructor);
        Task<InstructorResponseDto> DeleteInstructorByIdAsync(Guid id);

        public InstructorResponseDto MapToDTO(Instructor instructor);
    }
}
