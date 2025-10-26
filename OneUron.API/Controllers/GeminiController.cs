using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.FeatureDTOs;
using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;

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


        [HttpPost("quiz/generate")]
        [ProducesResponseType(typeof(ApiResponse<QuizResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateQuestions([FromBody] QuizRequestDto quiz)
        {
            var result = await _geminiService.GenerateQuestionsByQuizAsync(quiz);
            return Ok(ApiResponse<QuizResponseDto>.SuccessResponse(result, "Generated quiz successfully"));
        }

        //[HttpPost("schedule/{studyMethodId}/generate-tasks")]
        //public async Task<IActionResult> GenerateTasks(Guid studyMethodId, ScheduleRequestDto schedule)
        //{
        //    var result = await _geminiService.CreateTasksForScheduleAsync(studyMethodId, schedule);
        //    return Ok(ApiResponse<ScheduleResponeDto>.SuccessResponse(result, "Generated schedule tasks successfully"));
        //}

        [HttpPost("{userId:guid}/create-schedule")]
        [ProducesResponseType(typeof(ApiResponse<ScheduleResponeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateScheduleWithListSubjectAsync(Guid userId, [FromBody] ScheduleSubjectRequestDto scheduleSubject)
        {
            var result = await _geminiService.CreateScheduleWithListSubjectAsync(scheduleSubject, userId);
            return Ok(ApiResponse<ScheduleResponeDto>.SuccessResponse(result, "Create Schedule SuccessFully"));
        }

        [HttpPost("processId/{processId:guid}/tasks/generate")]
        [ProducesResponseType(typeof(ApiResponse<ProcessResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GenerateTasksForScheduleAsync(Guid processId,[FromBody] ProcessTaskGenerateRequest request)
        {
            var result = await _geminiService.CreatProcessTaskForProcessAsync(processId, request);
            return Ok(ApiResponse<ProcessResponseDto>.SuccessResponse(result,"Generate Task Successfully"));
        }
    }
}
