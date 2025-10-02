using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.EvaluationDTOs;
using OneUron.BLL.DTOs.UserAnswerDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAnswerController : Controller
    {
        private readonly IUserAnswerService _userAnswerService;

        public UserAnswerController(IUserAnswerService userAnswerService)
        {
            _userAnswerService = userAnswerService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _userAnswerService.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by-user")]
        public async Task<IActionResult> GetUserAnswerByUserIdAsync([FromQuery] Guid userId, [FromQuery] Guid evaluatioQuestionId)
        {
            var response = await _userAnswerService.GetByListUserAnswerAsync(userId, evaluatioQuestionId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("create-new")]
        public async Task<IActionResult> CreateNewUserAnswerAsync([FromBody] UserAnswerRequestDto request)
        {
            var response = await _userAnswerService.CreateNewUserAnswerAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by")]
        public async Task<IActionResult> UpdateUserAnswerByUserIdAsync([FromQuery] Guid id, [FromBody] UserAnswerUpdateRequestDto request)
        {
            var response = await _userAnswerService.UpdateUserAnswerByUserIdAsync(id, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("delete-by")]
        public async Task<IActionResult> DeleteUserAnswerByUserIdAsync([FromQuery] Guid id)
        {
            var response = await _userAnswerService.DeleteUserAnswerByAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("submit")]
        public async Task<IActionResult> SubmitEvaluationAsync([FromBody] List<EvaluationSubmitRequest> request)
        {
            var response = await _userAnswerService.SubmitAnswersAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
