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
        public Task<ApiResponse<List<CourseDetailResponseDto>>> GetAllCourseDetailAsync();

        public Task<ApiResponse<CourseDetailResponseDto>> GetCourseDetailbyIdAsync(Guid id);

        public  Task<ApiResponse<CourseDetailResponseDto>> CreateNewCourseDetailAsync(CourseDetailRequestDto request);

        public  Task<ApiResponse<CourseDetailResponseDto>> UpdateCourseDetailByIdAsync(Guid id, CourseDetailRequestDto newCourseDetail);

        public  Task<ApiResponse<CourseDetailResponseDto>> DeleteCourseDetailByIdAsync(Guid id);

    }
}
