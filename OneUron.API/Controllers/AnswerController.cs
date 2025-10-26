using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.AcknowledgeDTOs;
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
        [ProducesResponseType(typeof(ApiResponse<List<AnswerResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _answerService.GetAllAnswerAsync();
            return Ok(ApiResponse<List<AnswerResponseDto>>.SuccessResponse(result, "Get all answers successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<AnswerResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _answerService.GetAnswerByIdAsync(id);
            return Ok(ApiResponse<AnswerResponseDto>.SuccessResponse(result, "Get answer by id successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<AnswerResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] AnswerRequestDto request)
        {
            var result = await _answerService.CreateNewAnswerAsync(request);
            return Ok(ApiResponse<AnswerResponseDto>.SuccessResponse(result, "Create answer successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<AnswerResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] AnswerRequestDto request)
        {
            var result = await _answerService.UpdateAnswerByIdAsync(id, request);
            return Ok(ApiResponse<AnswerResponseDto>.SuccessResponse(result, "Update answer successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<AnswerResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _answerService.DeleteAnswerByIdAsync(id);
            return Ok(ApiResponse<AnswerResponseDto>.SuccessResponse(result, "Delete answer successfully"));
        }
    }
}
