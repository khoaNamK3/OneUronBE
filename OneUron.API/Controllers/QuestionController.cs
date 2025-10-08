using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.QuestionDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionController : Controller
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _questionService.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetbyIdAsync(Guid id)
        {
            var respone = await _questionService.GetbyIdAsync(id);

            if (!respone.Success)
            {
                return NotFound(respone);
            }
            return Ok(respone);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewQuestionAsync([FromBody] QuestionRequestDto request)
        {
            var respone = await _questionService.CreateNewQuestionAsync(request);

            if (!respone.Success)
            {
                return BadRequest(respone);
            }
            return Ok(respone);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateQuestionByIdAsync(Guid id, [FromBody] QuestionRequestDto request)
        {
            var response = await _questionService.UpdateQuestionByIdAsync(id, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteQuestionByIdAsync(Guid id)
        {
            var response  = await _questionService.DeleteQuestionByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
