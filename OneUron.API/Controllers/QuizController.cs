using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.DTOs.UserQuizAttemptDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Repository;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuizController : ControllerBase
    {
        private readonly IQuizService _quizService;

        public QuizController(IQuizService quizService)
        {
            _quizService = quizService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _quizService.GetAllQuizAsync();
            return Ok(ApiResponse<List<QuizResponseDto>>.SuccessResponse(result, "Get all quizzes successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _quizService.GetQuizByIdAsync(id);
            return Ok(ApiResponse<QuizResponseDto>.SuccessResponse(result, "Get quiz by ID successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(QuizRequestDto request)
        {
            var result = await _quizService.CreateNewQuizAsync(request);
            return Ok(ApiResponse<QuizResponseDto>.SuccessResponse(result, "Create quiz successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, QuizRequestDto request)
        {
            var result = await _quizService.UpdateQuizByIdAsync(id, request);
            return Ok(ApiResponse<QuizResponseDto>.SuccessResponse(result, "Update quiz successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _quizService.DeleteQuizByIdAsync(id);
            return Ok(ApiResponse<QuizResponseDto>.SuccessResponse(result, "Delete quiz successfully"));
        }

        [HttpGet("user/{userId:guid}/schedule/{scheduleId:guid}")]
        public async Task<IActionResult> GetUserScheduleInformationAsync(Guid userId, Guid scheduleId)
        {
            var result = await _quizService.GetUserScheduleInformationAsync(userId, scheduleId);
            return Ok(ApiResponse<UserScheduleInformationResponse>.SuccessResponse(result, "Get Information Successfully"));
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPagedQuizzesAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? name = null)
        {
            var result = await _quizService.GetPagedQuizzesAsync(pageNumber, pageSize, name);
            return Ok(ApiResponse<PagedResult<QuizResponseDto>>.SuccessResponse(result, "Paged quizzes retrieved successfully."));
        }

        [HttpGet("user/{userId:guid}/info")]
        public async Task<IActionResult> GetUserQuizInformationAsync(Guid userId)
        {
            var result = await _quizService.GetUserQuizInformation(userId);
            return Ok(ApiResponse<UserQuizInformationResponse>.SuccessResponse(result,"User quiz information retrieved successfully."));
        }

    }
}
