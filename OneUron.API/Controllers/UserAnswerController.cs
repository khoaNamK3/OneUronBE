using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _userAnswerService.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("by-user")]
        public async Task<IActionResult> GetUserAnswerByUserIdAsync([FromQuery]Guid userId, [FromQuery] Guid evaluatioQuestionId)
        {
            var response = await _userAnswerService.GetByListUserAnswerAsync(userId,evaluatioQuestionId);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewUserAnswerAsync([FromBody] UserAnswerRequestDto request)
        {
            var response = await _userAnswerService.CreateNewUserAnswerAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserAnswerByUserIdAsync([FromQuery] Guid id, [FromBody] UserAnswerUpdateRequestDto request )
        {
            var response = await _userAnswerService.UpdateUserAnswerByUserIdAsync(id, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUserAnswerByUserIdAsync([FromQuery] Guid id)
        {
            var response = await _userAnswerService.DeleteUserAnswerByAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
