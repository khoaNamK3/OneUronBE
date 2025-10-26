using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.EvaluationDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationController : ControllerBase
    {
        private readonly IEvaluationService _evaluationService;

        public EvaluationController(IEvaluationService evaluationService)
        {
            _evaluationService = evaluationService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<EvaluationResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _evaluationService.GetAllAsync();
            return Ok(ApiResponse<List<EvaluationResponseDto>>.SuccessResponse(result, "Get all evaluations successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<EvaluationResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _evaluationService.GetByIdAsync(id);
            return Ok(ApiResponse<EvaluationResponseDto>.SuccessResponse(result, "Get evaluation by ID successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<EvaluationResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] EvaluationRequestDto request)
        {
            var result = await _evaluationService.CreateNewEvaluationAsync(request);
            return Ok(ApiResponse<EvaluationResponseDto>.SuccessResponse(result, "Create evaluation successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<EvaluationResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] EvaluationRequestDto request)
        {
            var result = await _evaluationService.UpdateEvaluationByIdAsync(id, request);
            return Ok(ApiResponse<EvaluationResponseDto>.SuccessResponse(result, "Update evaluation successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<EvaluationResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _evaluationService.DeleteEvaluationByIdAsync(id);
            return Ok(ApiResponse<EvaluationResponseDto>.SuccessResponse(result, "Delete evaluation successfully"));
        }
    }
}
