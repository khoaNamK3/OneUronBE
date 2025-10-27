using FluentValidation;
using OneUron.BLL.DTOs.AcknowledgeDTOs;
using OneUron.BLL.DTOs.InstructorDTOs;
using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.DTOs.ResourceDTOs;
using OneUron.BLL.DTOs.SkillDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.ResourceRepo;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class ResourcesService : IResourcesService
    {
        private readonly IResourcesRepository _resourcesRepository;
        private readonly IAcknowledgeService _acknowledgeService;
        private readonly ISkillService _skillService;
        private readonly ICourseDetailService _courseDetailService;
        private readonly IInstructorService _instructorService;
        private readonly IValidator<ResourceRequestDto> _resourceRequestValidator;

        public ResourcesService(
            IResourcesRepository resourcesRepository,
            IAcknowledgeService acknowledgeService,
            ISkillService skillService,
            ICourseDetailService courseDetailService,
            IInstructorService instructorService,
            IValidator<ResourceRequestDto> resourceRequestValidator)
        {
            _resourcesRepository = resourcesRepository;
            _acknowledgeService = acknowledgeService;
            _skillService = skillService;
            _courseDetailService = courseDetailService;
            _instructorService = instructorService;
            _resourceRequestValidator = resourceRequestValidator;
        }

       
        public async Task<List<ResourceResponseDto>> GetAllResourceAsync()
        {
            var resources = await _resourcesRepository.GetAllResourceAsync();

            if (resources == null || !resources.Any())
                throw new ApiException.NotFoundException("Không tìm thấy kháo học nào.");

            return resources.Select(MapToDto).ToList();
        }

        
        public async Task<ResourceResponseDto> GetResourceByIdAsync(Guid id)
        {
            var resource = await _resourcesRepository.GetResourceByIdAsync(id);
            if (resource == null)
                throw new ApiException.NotFoundException($"Khóa học của ID {id} Không tìm thấy");

            return MapToDto(resource);
        }

  
        public async Task<ResourceResponseDto> CreateNewResourceAsync(ResourceRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Khóa học mới không được để trống");

            var validationResult = await _resourceRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newResource = MapToEntity(request);
            await _resourcesRepository.AddAsync(newResource);

            return MapToDto(newResource);
        }

     
        public async Task<ResourceResponseDto> UpdateResourceByIdAsync(Guid id, ResourceRequestDto request)
        {
            var existingResource = await _resourcesRepository.GetByIdAsync(id);
            if (existingResource == null)
                throw new ApiException.NotFoundException($"Khóa học của ID {id} Không tìm thấy.");

            if (request == null)
                throw new ApiException.BadRequestException("Khóa học mới không được để trống");

            var validationResult = await _resourceRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existingResource.Title = request.Title;
            existingResource.Organization = request.Organization;
            existingResource.Description = request.Description;
            existingResource.Star = request.Star;
            existingResource.Image = request.Image;
            existingResource.Reviews = request.Reviews;
            existingResource.Price = request.Price;
            existingResource.Type = request.Type;

            await _resourcesRepository.UpdateAsync(existingResource);

            return MapToDto(existingResource);
        }

       
        public async Task<ResourceResponseDto> DeleteResourceByIdAsync(Guid id)
        {
            var resource = await _resourcesRepository.GetByIdAsync(id);
            if (resource == null)
                throw new ApiException.NotFoundException($"Khóa học của ID {id} Không tìm thấy.");

            await _resourcesRepository.DeleteAsync(resource);
            return MapToDto(resource);
        }

        
        public ResourceResponseDto MapToDto(Resource r)
        {
            if (r == null) return null;

            return new ResourceResponseDto
            {
                Id = r.Id,
                Title = r.Title,
                Organization = r.Organization,
                Description = r.Description,
                Star = r.Star,
                Reviews = r.Reviews,
                Image = r.Image,
                Price = r.Price,
                Type = r.Type,

                courseDetail = _courseDetailService.MapToDto(r.CourseDetail),

                Acknowledges = r.Acknowledges?
                    .Select(a => _acknowledgeService.MapToDTO(a)).ToList()
                    ?? new List<AcknowledgeResponseDto>(),

                Instructors = r.Instructors?
                    .Select(i => _instructorService.MapToDTO(i)).ToList()
                    ?? new List<InstructorResponseDto>(),

                Skills = r.Skills?
                    .Select(s => _skillService.MapToDTO(s)).ToList()
                    ?? new List<SkillResponseDto>()
            };
        }

  
        protected Resource MapToEntity(ResourceRequestDto request)
        {
            return new Resource
            {
                Title = request.Title,
                Organization = request.Organization,
                Description = request.Description,
                Star = request.Star,
                Image = request.Image,
                Reviews = request.Reviews,
                Price = request.Price,
                Type = request.Type,

                Acknowledges = new List<Acknowledge>(),
                Instructors = new List<Instructor>(),
                Skills = new List<Skill>()
            };
        }
    }
}
