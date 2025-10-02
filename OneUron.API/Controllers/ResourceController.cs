using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.ResourceDTOs;
using OneUron.BLL.Interface;
using System.ComponentModel.Design;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : Controller
    {
        private readonly IResourcesService _resourceService;

        public ResourceController(IResourcesService resourceService)
        {
            _resourceService = resourceService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _resourceService.GetAllResourceAsync();
            if (!response.Success)
            {
               return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var response = await _resourceService.GetResourceByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("create-new")]
        public async Task<IActionResult> CreateNewResourceAsync([FromBody] ResourceRequestDto request)
        {
            var response = await _resourceService.CreateNewResourceAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateResourceById(Guid id, [FromBody] ResourceRequestDto request)
        {
            var response = await _resourceService.UpdateResourceByIdAsync(id, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteResourceById(Guid id)
        {
            var response = await _resourceService.DeletedResourceAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

    }
}
