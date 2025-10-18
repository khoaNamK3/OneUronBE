using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.ExceptionHandle;
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
        public async Task<IActionResult> GetAll()
        {
            var result = await _scheduleService.GetAllAsync();
            return Ok(ApiResponse<List<ScheduleResponeDto>>.SuccessResponse(result, "Get all schedules successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _scheduleService.GetByIdAsync(id);
            return Ok(ApiResponse<ScheduleResponeDto>.SuccessResponse(result, "Get schedule by ID successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(ScheduleRequestDto request)
        {
            var result = await _scheduleService.CreateScheduleAsync(request);
            return Ok(ApiResponse<ScheduleResponeDto>.SuccessResponse(result, "Create schedule successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, ScheduleRequestDto request)
        {
            var result = await _scheduleService.UpdateScheduleByIdAsync(id, request);
            return Ok(ApiResponse<ScheduleResponeDto>.SuccessResponse(result, "Update schedule successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _scheduleService.DeleteScheduleByIdAsync(id);
            return Ok(ApiResponse<ScheduleResponeDto>.SuccessResponse(result, "Delete schedule successfully"));
        }

        [HttpGet("schedule-week/{id:guid}")]
        public async Task<IActionResult> GetScheduleWeekInFormationAsync(Guid id)
        {
            var result = await _scheduleService.GetScheduleWeekInFormationAsync(id);
            return Ok(ApiResponse<ScheduleWeekRespone>.SuccessResponse(result, "Schedule week information retrieved successfully."));
        }

    }
}
