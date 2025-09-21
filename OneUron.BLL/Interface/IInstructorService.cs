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
        public  Task<ApiResponse<List<InstructorResponseDto>>> GetAllAsync();

        public  Task<ApiResponse<InstructorResponseDto>> GetInstructorByIdAsync(Guid id);

        public  Task<ApiResponse<InstructorResponseDto>> CreateNewInstructorAsync(InstructorRequestDto request);

        public  Task<ApiResponse<InstructorResponseDto>> UpdateInstructorByIdAsync(Guid id, InstructorRequestDto newInstructor);

        public  Task<ApiResponse<InstructorResponseDto>> DeleteInstructorByIdAsync(Guid id);

    }
}
