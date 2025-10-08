using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.AnswerDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswerController : Controller
    {
        private readonly IAnswerService _answerService;

        public AnswerController(IAnswerService answerService)
        {
            _answerService = answerService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _answerService.GetAllAnswerAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var respone = await _answerService.GetAnswerByIdAsyc(id);

            if (!respone.Success)
            {
                return NotFound(respone);
            }
            return Ok(respone);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAnswerAsync([FromBody] AnswerRequestDto request)
        {
            var response = await _answerService.CreateNewAnswerAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateAnswerByIdAsync(Guid id, [FromBody] AnswerRequestDto request)
        {
            var respone = await _answerService.UpdateAnswerByIdAsync(id, request);

            if (!respone.Success)
            {
                return BadRequest(respone);
            }
            return Ok(respone);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteAnswerByIdAsync(Guid id)
        {
            var response = await _answerService.DeleteAnswerByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

    }
}
