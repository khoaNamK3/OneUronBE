using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.QuestionChoiceDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionChoiceController : Controller
    {
        private readonly IQuestionChoiceService _questionChoiceService;


        public QuestionChoiceController(IQuestionChoiceService questionChoiceService)
        {
            _questionChoiceService = questionChoiceService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _questionChoiceService.GetAllQuestionChoiceAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var response = await _questionChoiceService.GetQuestionChoiceByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewQuestionChoiceAsync([FromBody] QuestionChoiceRequestDto request)
        {
            var response = await _questionChoiceService.CreateNewQuestionChoiceAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateQuestionChoiceByIdAsync(Guid id, QuestionChoiceRequestDto request)
        {
            var respone = await _questionChoiceService.UpdateQuestionChoiceByIdAsync(id, request);

            if (!respone.Success)
            {
                return BadRequest(respone);
            }
            return Ok(respone);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteQuestionChoiceByIdAsync(Guid id)
        {
            var response = await _questionChoiceService.DeleteQuestionChoiceByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
