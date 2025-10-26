using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.MethodProDTOs;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MethodRuleConditionController : ControllerBase
    {
        private readonly IMethodRuleConditionService _service;

        public MethodRuleConditionController(IMethodRuleConditionService service)
        {
            _service = service;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<MethodRuleConditionResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(ApiResponse<List<MethodRuleConditionResponseDto>>.SuccessResponse(result, "Get all MethodRuleCondition successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MethodRuleConditionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _service.GetByIdAsync(id);
            return Ok(ApiResponse<MethodRuleConditionResponseDto>.SuccessResponse(result, "Get MethodRuleCondition by ID successfully"));
        }

        [HttpGet("get-by-choice/{choiceId}")]
        [ProducesResponseType(typeof(ApiResponse<List<MethodRuleConditionResponseDto>?>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByChoiceId(Guid choiceId)
        {
            var result = await _service.GetMethodRuleConditionByChoiceId(choiceId);
            return Ok(ApiResponse<List<MethodRuleConditionResponseDto>?>.SuccessResponse(result, "Get MethodRuleCondition by choice ID successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<MethodRuleConditionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MethodRuleConditionRequestDto request)
        {
            var result = await _service.CreateNewMethodRuleConditionAsync(request);
            return Ok(ApiResponse<MethodRuleConditionResponseDto>.SuccessResponse(result, "Create MethodRuleCondition successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MethodRuleConditionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] MethodRuleConditionRequestDto request)
        {
            var result = await _service.UpdateMethodRuleConditionByIdAsync(id, request);
            return Ok(ApiResponse<MethodRuleConditionResponseDto>.SuccessResponse(result, "Update MethodRuleCondition successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MethodRuleConditionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _service.DeleteMethodRuleConditionByIdAsync(id);
            return Ok(ApiResponse<MethodRuleConditionResponseDto>.SuccessResponse(result, "Delete MethodRuleCondition successfully"));
        }
    }
}

