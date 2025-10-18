using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.AnswerDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _answerService.GetAllAnswerAsync();
            return Ok(ApiResponse<List<AnswerResponseDto>>.SuccessResponse(result, "Get all answers successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _answerService.GetAnswerByIdAsync(id);
            return Ok(ApiResponse<AnswerResponseDto>.SuccessResponse(result, "Get answer by id successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(AnswerRequestDto request)
        {
            var result = await _answerService.CreateNewAnswerAsync(request);
            return Ok(ApiResponse<AnswerResponseDto>.SuccessResponse(result, "Create answer successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, AnswerRequestDto request)
        {
            var result = await _answerService.UpdateAnswerByIdAsync(id, request);
            return Ok(ApiResponse<AnswerResponseDto>.SuccessResponse(result, "Update answer successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _answerService.DeleteAnswerByIdAsync(id);
            return Ok(ApiResponse<AnswerResponseDto>.SuccessResponse(result, "Delete answer successfully"));
        }
    }
}
