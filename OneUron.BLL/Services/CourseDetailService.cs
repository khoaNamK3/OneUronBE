using FluentValidation;
using OneUron.BLL.DTOs.ChoiceDTOs;
using OneUron.BLL.DTOs.CourseDetailDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
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
        private readonly IValidator<CourseDetailRequestDto> _courseDetailRequestValidator;

        public CourseDetailService(
            ICourseDetailRepository courseDetailRepository,
            IValidator<CourseDetailRequestDto> courseDetailRequestValidator)
        {
            _courseDetailRepository = courseDetailRepository;
            _courseDetailRequestValidator = courseDetailRequestValidator;
        }


        public async Task<List<CourseDetailResponseDto>> GetAllCourseDetailAsync()
        {
            var courseDetails = await _courseDetailRepository.GetAllCourseDetailAsync();

            if (courseDetails == null || !courseDetails.Any())
                throw new ApiException.NotFoundException("No course details found.");

            return courseDetails.Select(MapToDto).ToList();
        }

   
        public async Task<CourseDetailResponseDto> GetCourseDetailByIdAsync(Guid id)
        {
            var existCourseDetail = await _courseDetailRepository.GetCourseDetailbyIdAsync(id);

            if (existCourseDetail == null)
                throw new ApiException.NotFoundException($"CourseDetail with ID {id} not found.");

            return MapToDto(existCourseDetail);
        }

   
        public async Task<CourseDetailResponseDto> CreateNewCourseDetailAsync(CourseDetailRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("CourseDetail request cannot be null.");

            var validationResult = await _courseDetailRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newCourseDetail = MapToEntity(request);

            await _courseDetailRepository.AddAsync(newCourseDetail);

            return MapToDto(newCourseDetail);
        }

        
        public async Task<CourseDetailResponseDto> UpdateCourseDetailByIdAsync(Guid id, CourseDetailRequestDto newCourseDetail)
        {
            var existCourseDetail = await _courseDetailRepository.GetCourseDetailbyIdAsync(id);
            if (existCourseDetail == null)
                throw new ApiException.NotFoundException($"CourseDetail with ID {id} not found.");

            if (newCourseDetail == null)
                throw new ApiException.BadRequestException("New CourseDetail data cannot be null.");

            var validationResult = await _courseDetailRequestValidator.ValidateAsync(newCourseDetail);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existCourseDetail.Duration = newCourseDetail.Duration;
            existCourseDetail.Level = newCourseDetail.Level;
            existCourseDetail.Students = newCourseDetail.Students;
            existCourseDetail.Update = DateTime.UtcNow;
            existCourseDetail.Reviews = newCourseDetail.Reviews;
            existCourseDetail.Price = newCourseDetail.Price;
            existCourseDetail.ResourceId = newCourseDetail.ResourceId;

            await _courseDetailRepository.UpdateAsync(existCourseDetail);

            return MapToDto(existCourseDetail);
        }

       
        public async Task<CourseDetailResponseDto> DeleteCourseDetailByIdAsync(Guid id)
        {
            var existCourseDetail = await _courseDetailRepository.GetCourseDetailbyIdAsync(id);
            if (existCourseDetail == null)
                throw new ApiException.NotFoundException($"CourseDetail with ID {id} not found.");

            await _courseDetailRepository.DeleteAsync(existCourseDetail);

            return MapToDto(existCourseDetail);
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
