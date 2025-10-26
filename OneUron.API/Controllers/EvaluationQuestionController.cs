using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.EvaluationDTOs;
using OneUron.BLL.DTOs.EvaluationQuestionDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationQuestionController : ControllerBase
    {
        private readonly IEvaluationQuestionService _evaluationQuestionService;

        public EvaluationQuestionController(IEvaluationQuestionService evaluationQuestionService)
        {
            _evaluationQuestionService = evaluationQuestionService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<EvaluationQuestionResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _evaluationQuestionService.GetAllAsync();
            return Ok(ApiResponse<List<EvaluationQuestionResponseDto>>.SuccessResponse(result, "Get all evaluation questions successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<EvaluationQuestionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _evaluationQuestionService.GetByIdAsync(id);
            return Ok(ApiResponse<EvaluationQuestionResponseDto>.SuccessResponse(result, "Get evaluation question by id successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<EvaluationQuestionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] EvaluationQuestionRequestDto request)
        {
            var result = await _evaluationQuestionService.CreateNewEvaluationQuestionAsync(request);
            return Ok(ApiResponse<EvaluationQuestionResponseDto>.SuccessResponse(result, "Create evaluation question successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<EvaluationQuestionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] EvaluationQuestionRequestDto request)
        {
            var result = await _evaluationQuestionService.UpdateEvaluationQuestionByIdAsync(id, request);
            return Ok(ApiResponse<EvaluationQuestionResponseDto>.SuccessResponse(result, "Update evaluation question successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<EvaluationQuestionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _evaluationQuestionService.DeleteEvaluationQuestionByIdAsync(id);
            return Ok(ApiResponse<EvaluationQuestionResponseDto>.SuccessResponse(result, "Delete evaluation question successfully"));
        }
    }
}
