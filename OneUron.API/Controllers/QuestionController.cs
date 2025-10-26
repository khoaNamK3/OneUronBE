using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.QuestionChoiceDTOs;
using OneUron.BLL.DTOs.QuestionDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<QuestionResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _questionService.GetAllAsync();
            return Ok(ApiResponse<List<QuestionResponseDto>>.SuccessResponse(result, "Get all questions successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<QuestionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _questionService.GetByIdAsync(id);
            return Ok(ApiResponse<QuestionResponseDto>.SuccessResponse(result, "Get question by ID successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<QuestionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] QuestionRequestDto request)
        {
            var result = await _questionService.CreateNewQuestionAsync(request);
            return Ok(ApiResponse<QuestionResponseDto>.SuccessResponse(result, "Create question successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<QuestionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id,[FromBody] QuestionRequestDto request)
        {
            var result = await _questionService.UpdateQuestionByIdAsync(id, request);
            return Ok(ApiResponse<QuestionResponseDto>.SuccessResponse(result, "Update question successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<QuestionResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _questionService.DeleteQuestionByIdAsync(id);
            return Ok(ApiResponse<QuestionResponseDto>.SuccessResponse(result, "Delete question successfully"));
        }
    }
}
