using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MethodRuleController : ControllerBase
    {
        private readonly IMethodRuleService _service;

        public MethodRuleController(IMethodRuleService service)
        {
            _service = service;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(ApiResponse<List<MethodRuleResponseDto>>.SuccessResponse(result, "Get all MethodRule successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(ApiResponse<MethodRuleResponseDto>.SuccessResponse(result, "Get MethodRule by ID successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(MethodRuleRequestDto request)
        {
            var result = await _service.CreateNewMethodRuleAsync(request);
            return Ok(ApiResponse<MethodRuleResponseDto>.SuccessResponse(result, "Create MethodRule successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, MethodRuleRequestDto request)
        {
            var result = await _service.UpdateMethodRuleByIdAsync(id, request);
            return Ok(ApiResponse<MethodRuleResponseDto>.SuccessResponse(result, "Update MethodRule successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteMethodRuleByIdAsync(id);
            return Ok(ApiResponse<MethodRuleResponseDto>.SuccessResponse(result, "Delete MethodRule successfully"));
        }
    }
}
