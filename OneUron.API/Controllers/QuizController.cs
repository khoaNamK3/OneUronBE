using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.QuestionDTOs;
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
        [ProducesResponseType(typeof(ApiResponse<List<QuizResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _quizService.GetAllQuizAsync();
            return Ok(ApiResponse<List<QuizResponseDto>>.SuccessResponse(result, "Get all quizzes successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<QuizResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _quizService.GetQuizByIdAsync(id);
            return Ok(ApiResponse<QuizResponseDto>.SuccessResponse(result, "Get quiz by ID successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<QuizResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody]QuizRequestDto request)
        {
            var result = await _quizService.CreateNewQuizAsync(request);
            return Ok(ApiResponse<QuizResponseDto>.SuccessResponse(result, "Create quiz successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<QuizResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id,[FromBody] QuizRequestDto request)
        {
            var result = await _quizService.UpdateQuizByIdAsync(id, request);
            return Ok(ApiResponse<QuizResponseDto>.SuccessResponse(result, "Update quiz successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<QuizResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _quizService.DeleteQuizByIdAsync(id);
            return Ok(ApiResponse<QuizResponseDto>.SuccessResponse(result, "Delete quiz successfully"));
        }

        [HttpGet("user/{userId}/schedule/{scheduleId}")]
        [ProducesResponseType(typeof(ApiResponse<UserScheduleInformationResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserScheduleInformationAsync(Guid userId, Guid scheduleId)
        {
            var result = await _quizService.GetUserScheduleInformationAsync(userId, scheduleId);
            return Ok(ApiResponse<UserScheduleInformationResponse>.SuccessResponse(result, "Get Information Successfully"));
        }

        [HttpGet("paging")]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<QuizResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPagedQuizzesAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? name = null)
        {
            var result = await _quizService.GetPagedQuizzesAsync(pageNumber, pageSize, name);
            return Ok(ApiResponse<PagedResult<QuizResponseDto>>.SuccessResponse(result, "Paged quizzes retrieved successfully."));
        }

        [HttpGet("user/{userId}/info")]
        [ProducesResponseType(typeof(ApiResponse<UserQuizInformationResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUserQuizInformationAsync(Guid userId)
        {
            var result = await _quizService.GetUserQuizInformation(userId);
            return Ok(ApiResponse<UserQuizInformationResponse>.SuccessResponse(result,"User quiz information retrieved successfully."));
        }

        [HttpGet("paging-by-user/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<PagedResult<QuizResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByUser(Guid userId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _quizService.GetAllQuizByUserIdAsync(pageNumber, pageSize, userId);
            return Ok(ApiResponse<PagedResult<QuizResponseDto>>.SuccessResponse(result, "Get all quizzes successfully"));
        }

    }
}
