using OneUron.BLL.DTOs.CourseDetailDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface ICourseDetailService
    {

        Task<List<CourseDetailResponseDto>> GetAllCourseDetailAsync();
        Task<CourseDetailResponseDto> GetCourseDetailByIdAsync(Guid id);
        Task<CourseDetailResponseDto> CreateNewCourseDetailAsync(CourseDetailRequestDto request);
        Task<CourseDetailResponseDto> UpdateCourseDetailByIdAsync(Guid id, CourseDetailRequestDto newCourseDetail);
        Task<CourseDetailResponseDto> DeleteCourseDetailByIdAsync(Guid id);

        public CourseDetailResponseDto MapToDto(CourseDetail courseDetail);

    }
}
