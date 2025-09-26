using OneUron.BLL.DTOs.AcknowledgeDTOs;
using OneUron.BLL.DTOs.InstructorDTOs;
using OneUron.BLL.DTOs.ResourceDTOs;
using OneUron.BLL.DTOs.SkillDTOs;
using OneUron.BLL.ExceptionHandle;
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
        public ResourcesService(IResourcesRepository resourcesRepository, IAcknowledgeService acknowledgeService, ISkillService skillService, ICourseDetailService courseDetailService, IInstructorService instructorService)
        {
            _resourcesRepository = resourcesRepository;
            _acknowledgeService = acknowledgeService;
            _skillService = skillService;
            _courseDetailService = courseDetailService;
            _instructorService = instructorService;
        }

        public async Task<ApiResponse<List<ResourceResponseDto>>> GetAllResourceAsync()
        {
            try
            {
                var resources = await _resourcesRepository.GetAllResourceAsync();

                if (!resources.Any())
                {
                    return ApiResponse<List<ResourceResponseDto>>.FailResponse("Get All fail", "Resource Are Empty");
                }


                var result = resources.Select(MapToDto).ToList();


                return ApiResponse<List<ResourceResponseDto>>.SuccessResponse(result, "Get Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ResourceResponseDto>>.FailResponse("Get All Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ResourceResponseDto>> GetResourceByIdAsync(Guid id)
        {
            try
            {
                var resource = await _resourcesRepository.GetResourceByIdAsync(id);

                if (resource == null)
                {
                    return ApiResponse<ResourceResponseDto>.FailResponse("Get by Id fail", "Resource are not exists");
                }

                var result = MapToDto(resource);
                return ApiResponse<ResourceResponseDto>.SuccessResponse(result, "Get Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ResourceResponseDto>.FailResponse("Get by Id fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ResourceResponseDto>> CreateNewResourceAsync(ResourceRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<ResourceResponseDto>.FailResponse("Create fail", "resource is Null");
                }

                var newResource = MapToEntity(request);

                await _resourcesRepository.AddAsync(newResource);

                var result = MapToDto(newResource);

                return ApiResponse<ResourceResponseDto>.SuccessResponse(result, "Create Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ResourceResponseDto>.FailResponse("Create Fail", ex.Message);
            }

        }

        public async Task<ApiResponse<ResourceResponseDto>> UpdateResourceByIdAsync(Guid id, ResourceRequestDto request)
        {
            try
            {
                var exitResource = await _resourcesRepository.GetByIdAsync(id);
                if (exitResource == null)
                {
                    return ApiResponse<ResourceResponseDto>.FailResponse("Update fail", "resource Are not exists");
                }

                if (request == null)
                {
                    return ApiResponse<ResourceResponseDto>.FailResponse("Update fail", "Request is Null");
                }

               
                exitResource.Title = request.Title;
                exitResource.Organization = request.Organization;
                exitResource.Description = request.Description;
                exitResource.Star = request.Star;
                exitResource.Image = request.Image;
                exitResource.Reviews = request.Reviews;
                exitResource.Price = request.Price;
                exitResource.Type = request.Type;

                await _resourcesRepository.UpdateAsync(exitResource);

                var result = MapToDto(exitResource);
                return ApiResponse<ResourceResponseDto>.SuccessResponse(result, "Update successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ResourceResponseDto>.FailResponse("Update fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ResourceResponseDto>> DeletedResourceAsync(Guid id)
        {
            try
            {
                var exitResource = await _resourcesRepository.GetByIdAsync(id);
                if (exitResource == null)
                {
                    return ApiResponse<ResourceResponseDto>.FailResponse("Delete fail", "Resource are not exists");
                }
                var result = MapToDto(exitResource);

                await _resourcesRepository.DeleteAsync(exitResource);

                return ApiResponse<ResourceResponseDto>.SuccessResponse(result, "Delete successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ResourceResponseDto>.FailResponse("Delete fail", ex.Message);
            }
        }

        public ResourceResponseDto MapToDto(Resource r)
        {
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

                Acknowledges = r.Acknowledges?.Select(a => _acknowledgeService.MapToDTO(a)).ToList()
                                ?? new List<AcknowledgeResponseDto>(),

                Instructors = r.Instructors?.Select(i => _instructorService.MapToDTO(i)).ToList()
                                ?? new List<InstructorResponseDto>(),

                Skills = r.Skills?.Select(s => _skillService.MapToDTO(s)).ToList()
                                ?? new List<SkillResponseDto>()
            };
        }

        protected Resource MapToEntity(ResourceRequestDto request)
        {
            return new Resource
            {
                Id = Guid.NewGuid(),
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
