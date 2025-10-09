using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.DTOs.ScheduleDTOs;
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

        [HttpPost("study-method/{studyMethodId}/schedules/tasks")]
        public async Task<IActionResult> CreateTaskForScheduleFollowStudyMethodIdAsync(Guid studyMethodId, [FromBody] ScheduleRequestDto newSchedule)
        {
            var response = await _geminiService.CreateTaskForScheduleFollowStudyMethodIdAsync(studyMethodId, newSchedule);

            if (!response.Success) { 
            return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
