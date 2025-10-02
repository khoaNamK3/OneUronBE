using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.EnRollDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnRollController : Controller
    {
        private readonly IEnRollService _enrollService;

        public EnRollController(IEnRollService enRollService)
        {
            _enrollService = enRollService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _enrollService.GetAllEnRollAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var response = await _enrollService.GetEnRollByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("create-new")]
        public async Task<IActionResult> CreateNewEnRollAsync([FromBody] EnRollRequestDto request)
        {
            var response = await _enrollService.CreateNewEnRollAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateEnRollByIdAsync(Guid id, [FromBody] EnRollRequestDto request)
        {
            var response = await _enrollService.UpdateEnRollByIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteEnRollByIdAsync(Guid id)
        {
            var response = await _enrollService.DeleteEnRollByIdAsync(id);
            if (!response.Success) 
            { 
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
