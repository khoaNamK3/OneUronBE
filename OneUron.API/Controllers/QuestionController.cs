using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> GetAll()
        {
            var result = await _questionService.GetAllAsync();
            return Ok(ApiResponse<List<QuestionResponseDto>>.SuccessResponse(result, "Get all questions successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _questionService.GetByIdAsync(id);
            return Ok(ApiResponse<QuestionResponseDto>.SuccessResponse(result, "Get question by ID successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(QuestionRequestDto request)
        {
            var result = await _questionService.CreateNewQuestionAsync(request);
            return Ok(ApiResponse<QuestionResponseDto>.SuccessResponse(result, "Create question successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, QuestionRequestDto request)
        {
            var result = await _questionService.UpdateQuestionByIdAsync(id, request);
            return Ok(ApiResponse<QuestionResponseDto>.SuccessResponse(result, "Update question successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _questionService.DeleteQuestionByIdAsync(id);
            return Ok(ApiResponse<QuestionResponseDto>.SuccessResponse(result, "Delete question successfully"));
        }
    }
}
