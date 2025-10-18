using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.ResourceDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using System.ComponentModel.Design;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourcesController : ControllerBase
    {
        private readonly IResourcesService _resourcesService;

        public ResourcesController(IResourcesService resourcesService)
        {
            _resourcesService = resourcesService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _resourcesService.GetAllResourceAsync();
            return Ok(ApiResponse<List<ResourceResponseDto>>.SuccessResponse(result, "Get all resources successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _resourcesService.GetResourceByIdAsync(id);
            return Ok(ApiResponse<ResourceResponseDto>.SuccessResponse(result, "Get resource successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(ResourceRequestDto request)
        {
            var result = await _resourcesService.CreateNewResourceAsync(request);
            return Ok(ApiResponse<ResourceResponseDto>.SuccessResponse(result, "Create resource successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, ResourceRequestDto request)
        {
            var result = await _resourcesService.UpdateResourceByIdAsync(id, request);
            return Ok(ApiResponse<ResourceResponseDto>.SuccessResponse(result, "Update resource successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _resourcesService.DeleteResourceByIdAsync(id);
            return Ok(ApiResponse<ResourceResponseDto>.SuccessResponse(result, "Delete resource successfully"));
        }
    }
}
