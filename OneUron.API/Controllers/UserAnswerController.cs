using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.EvaluationDTOs;
using OneUron.BLL.DTOs.UserAnswerDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAnswerController : ControllerBase
    {
        private readonly IUserAnswerService _userAnswerService;

        public UserAnswerController(IUserAnswerService userAnswerService)
        {
            _userAnswerService = userAnswerService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userAnswerService.GetAllAsync();
            return Ok(ApiResponse<List<UserAnswerResponseDto>>.SuccessResponse(result, "Get all user answers successfully"));
        }

        [HttpGet("get-list-by/{userId}/{questionId}")]
        public async Task<IActionResult> GetByList(Guid userId, Guid questionId)
        {
            var result = await _userAnswerService.GetByListAsync(userId, questionId);
            return Ok(ApiResponse<List<UserAnswerResponseDto>>.SuccessResponse(result, "Get user answers successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(UserAnswerRequestDto request)
        {
            var result = await _userAnswerService.CreateAsync(request);
            return Ok(ApiResponse<UserAnswerResponseDto>.SuccessResponse(result, "Create user answer successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, UserAnswerUpdateRequestDto request)
        {
            var result = await _userAnswerService.UpdateByIdAsync(id, request);
            return Ok(ApiResponse<UserAnswerResponseDto>.SuccessResponse(result, "Update user answer successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userAnswerService.DeleteByIdAsync(id);
            return Ok(ApiResponse<UserAnswerResponseDto>.SuccessResponse(result, "Delete user answer successfully"));
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitAnswers(List<EvaluationSubmitRequest> evaluations)
        {
            var result = await _userAnswerService.SubmitAnswersAsync(evaluations);
            return Ok(ApiResponse<List<UserAnswerResponseDto>>.SuccessResponse(result, "Submit answers successfully"));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAllByUser(Guid userId)
        {
            var result = await _userAnswerService.GetAllByUserIdAsync(userId);
            return Ok(ApiResponse<List<UserAnswerResponseDto>>.SuccessResponse(result, "Get all answers by user successfully"));
        }
    }
}
