using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
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
        [ProducesResponseType(typeof(ApiResponse<List<MethodRuleResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(ApiResponse<List<MethodRuleResponseDto>>.SuccessResponse(result, "Get all MethodRule successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MethodRuleResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(ApiResponse<MethodRuleResponseDto>.SuccessResponse(result, "Get MethodRule by ID successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<MethodRuleResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MethodRuleRequestDto request)
        {
            var result = await _service.CreateNewMethodRuleAsync(request);
            return Ok(ApiResponse<MethodRuleResponseDto>.SuccessResponse(result, "Create MethodRule successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MethodRuleResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id,[FromBody] MethodRuleRequestDto request)
        {
            var result = await _service.UpdateMethodRuleByIdAsync(id, request);
            return Ok(ApiResponse<MethodRuleResponseDto>.SuccessResponse(result, "Update MethodRule successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MethodRuleResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteMethodRuleByIdAsync(id);
            return Ok(ApiResponse<MethodRuleResponseDto>.SuccessResponse(result, "Delete MethodRule successfully"));
        }
    }
}
