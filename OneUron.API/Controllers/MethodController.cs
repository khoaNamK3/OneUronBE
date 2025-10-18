using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.MethodDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.BLL.Services;

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
        public async Task<IActionResult> GetAll()
        {
            var result = await _methodService.GetAllAsync();
            return Ok(ApiResponse<List<MethodResponseDto>>.SuccessResponse(result, "Get all methods successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _methodService.GetByIdAsync(id);
            return Ok(ApiResponse<MethodResponseDto>.SuccessResponse(result, "Get method by ID successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(MethodRequestDto request)
        {
            var result = await _methodService.CreateNewMethodAsync(request);
            return Ok(ApiResponse<MethodResponseDto>.SuccessResponse(result, "Create method successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, MethodRequestDto request)
        {
            var result = await _methodService.UpdateMethodByIdAsync(id, request);
            return Ok(ApiResponse<MethodResponseDto>.SuccessResponse(result, "Update method successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _methodService.DeleteMethodByIdAsync(id);
            return Ok(ApiResponse<MethodResponseDto>.SuccessResponse(result, "Delete method successfully"));
        }

        [HttpGet("top3/{userId}")]
        public async Task<IActionResult> GetTop3(Guid userId)
        {
            var result = await _methodService.GetTop3MethodForUserAsync(userId);
            return Ok(ApiResponse<List<MethodSuggestionRespone>>.SuccessResponse(result, "Get top 3 methods for user successfully"));
        }
    }
}
