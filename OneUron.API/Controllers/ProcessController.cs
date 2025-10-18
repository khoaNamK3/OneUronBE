using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProcessController : ControllerBase
    {
        private readonly IProcessService _processService;

        public ProcessController(IProcessService processService)
        {
            _processService = processService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _processService.GetAllAsync();
            return Ok(ApiResponse<List<ProcessResponseDto>>.SuccessResponse(result, "Get all processes successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _processService.GetByIdAsync(id);
            return Ok(ApiResponse<ProcessResponseDto>.SuccessResponse(result, "Get process by ID successfully"));
        }

        [HttpGet("get-process-by/{scheduleId}")]
        public async Task<IActionResult> GetAllProcessesByScheduleIdAsync(Guid scheduleId) { 
        var result = await _processService.GetProcessesByScheduleId(scheduleId);
            return Ok(ApiResponse<List<ProcessResponseDto>>.SuccessResponse(result, "Get all processes successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(ProcessRequestDto request)
        {
            var result = await _processService.CreateProcessAsync(request);
            return Ok(ApiResponse<ProcessResponseDto>.SuccessResponse(result, "Create process successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, ProcessRequestDto request)
        {
            var result = await _processService.UpdateProcessByIdAsync(id, request);
            return Ok(ApiResponse<ProcessResponseDto>.SuccessResponse(result, "Update process successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _processService.DeleteProcessByIdAsync(id);
            return Ok(ApiResponse<ProcessResponseDto>.SuccessResponse(result, "Delete process successfully"));
        }

        
    }
}
