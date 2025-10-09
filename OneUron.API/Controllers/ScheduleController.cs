using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;

        public ScheduleController(IScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _scheduleService.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetbyIdAsync(Guid id)
        {
            var respone = await _scheduleService.GetByIdAsync(id);

            if (!respone.Success)
            {
                return NotFound(respone);
            }
            return Ok(respone);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateScheduleAsync([FromBody] ScheduleRequestDto request)
        {
            var response = await _scheduleService.CreateScheduleAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateScheduleByIdAsync(Guid id, [FromBody] ScheduleRequestDto request)
        {
            var response = await _scheduleService.UpdateScheduleByIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteScheduleByIdAsync(Guid id)
        {
            var response = await _scheduleService.DeleteScheduleByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
