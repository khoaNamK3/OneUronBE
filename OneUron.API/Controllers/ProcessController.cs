using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.PaymentDTOs;
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
        [ProducesResponseType(typeof(ApiResponse<List<ProcessResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _processService.GetAllAsync();
            return Ok(ApiResponse<List<ProcessResponseDto>>.SuccessResponse(result, "Get all processes successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProcessResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _processService.GetByIdAsync(id);
            return Ok(ApiResponse<ProcessResponseDto>.SuccessResponse(result, "Get process by ID successfully"));
        }

        [HttpGet("get-process-by/{scheduleId}")]
        [ProducesResponseType(typeof(ApiResponse<List<ProcessResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProcessesByScheduleIdAsync(Guid scheduleId) { 
        var result = await _processService.GetProcessesByScheduleId(scheduleId);
            return Ok(ApiResponse<List<ProcessResponseDto>>.SuccessResponse(result, "Get all processes successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<ProcessResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody]ProcessRequestDto request)
        {
            var result = await _processService.CreateProcessAsync(request);
            return Ok(ApiResponse<ProcessResponseDto>.SuccessResponse(result, "Create process successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProcessResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id,[FromBody] ProcessRequestDto request)
        {
            var result = await _processService.UpdateProcessByIdAsync(id, request);
            return Ok(ApiResponse<ProcessResponseDto>.SuccessResponse(result, "Update process successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProcessResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _processService.DeleteProcessByIdAsync(id);
            return Ok(ApiResponse<ProcessResponseDto>.SuccessResponse(result, "Delete process successfully"));
        }

        
    }
}
