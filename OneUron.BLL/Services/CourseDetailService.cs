using OneUron.BLL.DTOs.CourseDetailDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.CourseDetailRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class CourseDetailService : ICourseDetailService
    {
        private readonly ICourseDetailRepository _courseDetailRepository;

        public CourseDetailService(ICourseDetailRepository courseDetailRepository)
        {
            _courseDetailRepository = courseDetailRepository;
        }

        public async Task<ApiResponse<List<CourseDetailResponseDto>>> GetAllCourseDetailAsync()
        {
            try
            {
                var courseDetails = await _courseDetailRepository.GetAllCourseDetailAsync();

                if (!courseDetails.Any())
                {
                    return ApiResponse<List<CourseDetailResponseDto>>.FailResponse("Get All CourseDetail Fail", "CourseDetail Are Empty");
                }

                var results = courseDetails.Select(MapToDto).ToList();

                return ApiResponse<List<CourseDetailResponseDto>>.SuccessResponse(results, "Get All CourseDetail Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CourseDetailResponseDto>>.FailResponse("Get All CourseDetail Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<CourseDetailResponseDto>> GetCourseDetailbyIdAsync(Guid id)
        {
            try
            {
                var existCourseDetail = await _courseDetailRepository.GetCourseDetailbyIdAsync(id);

                if (existCourseDetail == null)
                {
                    return ApiResponse<CourseDetailResponseDto>.FailResponse("Get CourseDetail By Id Fail", "CourseDetail Are Not Exist");
                }

                var result = MapToDto(existCourseDetail);

                return ApiResponse<CourseDetailResponseDto>.SuccessResponse(result, "Get CourseDetail By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CourseDetailResponseDto>.FailResponse("Get CourseDetail By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<CourseDetailResponseDto>> CreateNewCourseDetailAsync(CourseDetailRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<CourseDetailResponseDto>.FailResponse("Create New CourseDetail Fail", "CourseDetail is Null");
                }

                var newCourseDetail = MapToEntity(request);

                await _courseDetailRepository.AddAsync(newCourseDetail);

                var result = MapToDto(newCourseDetail);

                return ApiResponse<CourseDetailResponseDto>.SuccessResponse(result, "Create New CourseDetail Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CourseDetailResponseDto>.FailResponse("Create New CourseDetail Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<CourseDetailResponseDto>> UpdateCourseDetailByIdAsync(Guid id, CourseDetailRequestDto newCourseDetail)
        {
            try
            {
                var existCourseDetail = await _courseDetailRepository.GetCourseDetailbyIdAsync(id);

                if (existCourseDetail == null)
                {
                    return ApiResponse<CourseDetailResponseDto>.FailResponse("Update CourseDetail Fail", "CourseDetail Are Not Exist");
                }

                if (newCourseDetail == null)
                {
                    return ApiResponse<CourseDetailResponseDto>.FailResponse("Update CourseDetail Fail", "New CourseDetail is Null");
                }

                existCourseDetail.Duration = newCourseDetail.Duration;
                existCourseDetail.Level = newCourseDetail.Level;
                existCourseDetail.Students = newCourseDetail.Students;
                existCourseDetail.Update = DateTime.UtcNow;
                existCourseDetail.Reviews = newCourseDetail.Reviews;
                existCourseDetail.Price = newCourseDetail.Price;
                existCourseDetail.ResourceId = newCourseDetail.ResourceId;

                await _courseDetailRepository.UpdateAsync(existCourseDetail);

                var result = MapToDto(existCourseDetail);

                return ApiResponse<CourseDetailResponseDto>.SuccessResponse(result, "Update CourseDetail Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CourseDetailResponseDto>.FailResponse("Update CourseDetail Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<CourseDetailResponseDto>> DeleteCourseDetailByIdAsync(Guid id)
        {
            try
            {
                var existCourseDetail = await _courseDetailRepository.GetCourseDetailbyIdAsync(id);

                if (existCourseDetail == null)
                {
                    return ApiResponse<CourseDetailResponseDto>.FailResponse("Delete CourseDetail Fail", "CourseDetail Are Not Exist");
                }
                var result = MapToDto(existCourseDetail);
                await _courseDetailRepository.DeleteAsync(existCourseDetail);

                return ApiResponse<CourseDetailResponseDto>.SuccessResponse(result, "Delete CourseDetail Successfully");
                
            }
            catch (Exception ex)
            {
                return ApiResponse<CourseDetailResponseDto>.FailResponse("Delete CourseDetail Fail",ex.Message);
            }
        }

        protected CourseDetail MapToEntity(CourseDetailRequestDto courseDetailRequest)
        {
            return new CourseDetail
            {
                Duration = courseDetailRequest.Duration,
                Level = courseDetailRequest.Level,
                Students = courseDetailRequest.Students,
                Reviews = courseDetailRequest.Reviews,
                Price = courseDetailRequest.Price,
                ResourceId = courseDetailRequest.ResourceId
            };
        }

        public CourseDetailResponseDto MapToDto(CourseDetail courseDetail)
        {
            if (courseDetail == null) return null; 

            return new CourseDetailResponseDto
            {
                Id = courseDetail.Id,
                Duration = courseDetail.Duration,
                Level = courseDetail.Level,
                Students = courseDetail.Students,
                Update = courseDetail.Update,
                Reviews = courseDetail.Reviews,
                Price = courseDetail.Price,
                ResourceId = courseDetail.ResourceId
            };
        }

    }
}
