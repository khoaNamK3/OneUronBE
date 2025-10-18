using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.MethodProDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.BLL.Services;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MethodProController : ControllerBase
    {
        private readonly IMethodProSerivce _methodProService;

        public MethodProController(IMethodProSerivce methodProService)
        {
            _methodProService = methodProService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _methodProService.GetAllAsync();
            return Ok(ApiResponse<List<MethodProResponseDto>>.SuccessResponse(result, "Get all MethodPro successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _methodProService.GetByIdAsync(id);
            return Ok(ApiResponse<MethodProResponseDto>.SuccessResponse(result, "Get MethodPro by id successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(MethodProRequestDto request)
        {
            var result = await _methodProService.CreateNewMethodProAsync(request);
            return Ok(ApiResponse<MethodProResponseDto>.SuccessResponse(result, "Create MethodPro successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, MethodProRequestDto request)
        {
            var result = await _methodProService.UpdateMethodProByIdAsync(id, request);
            return Ok(ApiResponse<MethodProResponseDto>.SuccessResponse(result, "Update MethodPro successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _methodProService.DeleteMethodProByIdAsync(id);
            return Ok(ApiResponse<MethodProResponseDto>.SuccessResponse(result, "Delete MethodPro successfully"));
        }
    }
}
