using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.DTOs.QuestionChoiceDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionChoiceController : ControllerBase
    {
        private readonly IQuestionChoiceService _questionChoiceService;

        public QuestionChoiceController(IQuestionChoiceService questionChoiceService)
        {
            _questionChoiceService = questionChoiceService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<QuestionChoiceReponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _questionChoiceService.GetAllQuestionChoiceAsync();
            return Ok(ApiResponse<List<QuestionChoiceReponseDto>>.SuccessResponse(result, "Get all QuestionChoices successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<QuestionChoiceReponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _questionChoiceService.GetQuestionChoiceByIdAsync(id);
            return Ok(ApiResponse<QuestionChoiceReponseDto>.SuccessResponse(result, "Get QuestionChoice by ID successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<QuestionChoiceReponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody]QuestionChoiceRequestDto request)
        {
            var result = await _questionChoiceService.CreateNewQuestionChoiceAsync(request);
            return Ok(ApiResponse<QuestionChoiceReponseDto>.SuccessResponse(result, "Create QuestionChoice successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<QuestionChoiceReponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id,[FromBody] QuestionChoiceRequestDto request)
        {
            var result = await _questionChoiceService.UpdateQuestionChoiceByIdAsync(id, request);
            return Ok(ApiResponse<QuestionChoiceReponseDto>.SuccessResponse(result, "Update QuestionChoice successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<QuestionChoiceReponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _questionChoiceService.DeleteQuestionChoiceByIdAsync(id);
            return Ok(ApiResponse<QuestionChoiceReponseDto>.SuccessResponse(result, "Delete QuestionChoice successfully"));
        }
    }
}
