using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.UserQuizAttemptDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserQuizAttemptController : ControllerBase
    {
        private readonly IUserQuizAttemptService _userQuizAttemptService;

        public UserQuizAttemptController(IUserQuizAttemptService userQuizAttemptService)
        {
            _userQuizAttemptService = userQuizAttemptService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userQuizAttemptService.GetAllAsync();
            return Ok(ApiResponse<List<UserQuizAttemptResponseDto>>.SuccessResponse(result, "Get all quiz attempts successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _userQuizAttemptService.GetByIdAsync(id);
            return Ok(ApiResponse<UserQuizAttemptResponseDto>.SuccessResponse(result, "Get quiz attempt successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(UserQuizAttemptRequestDto request)
        {
            var result = await _userQuizAttemptService.CreateAsync(request);
            return Ok(ApiResponse<UserQuizAttemptResponseDto>.SuccessResponse(result, "Create quiz attempt successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, UserQuizAttemptRequestDto request)
        {
            var result = await _userQuizAttemptService.UpdateByIdAsync(id, request);
            return Ok(ApiResponse<UserQuizAttemptResponseDto>.SuccessResponse(result, "Update quiz attempt successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userQuizAttemptService.DeleteByIdAsync(id);
            return Ok(ApiResponse<UserQuizAttemptResponseDto>.SuccessResponse(result, "Delete quiz attempt successfully"));
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitAnswerAsync([FromBody] SubmitAnswerRequest request)
        {
            var result = await _userQuizAttemptService.SubmitAnswerAsync(request);
            return Ok(ApiResponse<UserQuizAttemptResponseDto>.SuccessResponse( result,"User quiz submitted successfully and result calculated."));
        }


    }
}
