using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeminiController : Controller
    {
        private readonly IGeminiService _geminiService;

        public GeminiController(IGeminiService geminiService)
        {
            _geminiService = geminiService;
        }


        [HttpPost("generate-question")]
        public async Task<IActionResult> GenerateQuestions([FromBody] QuizRequestDto request)
        {
            var response = await _geminiService.GenerateQuestionByQuizIdAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
