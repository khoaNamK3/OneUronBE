using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.UserQuizAttemptDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserQuizAttemptController : Controller
    {
        private readonly IUserQuizAttemptService _userQuizAttemptService;

        public UserQuizAttemptController(IUserQuizAttemptService userQuizAttemptService)
        {
            _userQuizAttemptService = userQuizAttemptService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _userQuizAttemptService.GetAllUserQuizAttemptAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var response = await _userQuizAttemptService.GetUserQuizAttemptsByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewAsync([FromBody] UserQuizAttemptRequestDto request)
        {
            var response = await _userQuizAttemptService.CreateNewUserQuizAttemptAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateByIdAsyc(Guid id, [FromBody] UserQuizAttemptRequestDto request)
        {
            var response = await _userQuizAttemptService.UpdateUserQuizAttemptByIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteByIdAsync(Guid id)
        {
            var response = await _userQuizAttemptService.DeleteUserQuizAttemptByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

    }
}
