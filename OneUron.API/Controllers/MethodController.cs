using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.MethodConDTOs;
using OneUron.BLL.DTOs.MethodDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.BLL.Services;
using OneUron.DAL.Repository;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MethodController : ControllerBase
    {
        private readonly IMethodSerivce _methodService;

        public MethodController(IMethodSerivce methodService)
        {
            _methodService = methodService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<MethodResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _methodService.GetAllAsync();
            return Ok(ApiResponse<List<MethodResponseDto>>.SuccessResponse(result, "Get all methods successfully"));
        }

        [HttpGet("get-paging")]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<MethodPagingResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPagingMethodAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? name = null)
        {
            var response = await _methodService.GetMethodPagingAsync(pageNumber, pageSize, name);
            return Ok(ApiResponse<PagedResult<MethodPagingResponse>>.SuccessResponse(response, "Get method paging Sucessfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MethodResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _methodService.GetByIdAsync(id);
            return Ok(ApiResponse<MethodResponseDto>.SuccessResponse(result, "Get method by ID successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<MethodResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MethodRequestDto request)
        {
            var result = await _methodService.CreateNewMethodAsync(request);
            return Ok(ApiResponse<MethodResponseDto>.SuccessResponse(result, "Create method successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MethodResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] MethodRequestDto request)
        {
            var result = await _methodService.UpdateMethodByIdAsync(id, request);
            return Ok(ApiResponse<MethodResponseDto>.SuccessResponse(result, "Update method successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MethodResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _methodService.DeleteMethodByIdAsync(id);
            return Ok(ApiResponse<MethodResponseDto>.SuccessResponse(result, "Delete method successfully"));
        }

        [HttpGet("top3/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<List<MethodSuggestionRespone>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetTop3(Guid userId)
        {
            var result = await _methodService.GetTop3MethodForUserAsync(userId);
            return Ok(ApiResponse<List<MethodSuggestionRespone>>.SuccessResponse(result, "Get top 3 methods for user successfully"));
        }
    }
}
