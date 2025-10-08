using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : Controller
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _quizService.GetAllQuizAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var response = await _quizService.GetQuizByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateNewQuizAsync([FromBody] QuizRequestDto request)
        {
            var response = await _quizService.CreateNewQuizAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        // fromRoute : id will see on the URL 
        // formQuery : id will see on the next "?"

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateQuizByIdAsync([FromRoute] Guid id, [FromBody] QuizRequestDto request)
        {
            var response = await _quizService.UpdateQuizByIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteQuizByIdAsync(Guid id)
        {
            var response = await _quizService.DeleteQuizByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
