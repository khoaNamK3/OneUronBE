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
        public Task<ApiResponse<List<StudyMethodResponseDto>>> GetALlAsync();

        public Task<ApiResponse<StudyMethodResponseDto>> GetByIdAsyc(Guid id);

        public  Task<ApiResponse<StudyMethodResponseDto>> CreateNewStudyMethodAsync(StudyMethodRequestDto request);

        public  Task<ApiResponse<StudyMethodResponseDto>> UpdateStudyMethodbyIdAsync(Guid id, StudyMethodRequestDto newStudyMethod);

        public  Task<ApiResponse<StudyMethodResponseDto>> DeleteStudyMethodbyIdAsync(Guid id);


    }
}
