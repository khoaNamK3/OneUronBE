using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessTaskController : ControllerBase
    {
        private readonly IProcessTaskService _processTaskService;

        public ProcessTaskController(IProcessTaskService processTaskService)
        {
            _processTaskService = processTaskService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<ProcessTaskResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _processTaskService.GetAllAsync();
            return Ok(ApiResponse<List<ProcessTaskResponseDto>>.SuccessResponse(result, "Get all process tasks successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProcessTaskResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _processTaskService.GetByIdAsync(id);
            return Ok(ApiResponse<ProcessTaskResponseDto>.SuccessResponse(result, "Get process task by ID successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<ProcessTaskResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody]ProcessTaskRequestDto request)
        {
            var result = await _processTaskService.CreateProcessTaskAsync(request);
            return Ok(ApiResponse<ProcessTaskResponseDto>.SuccessResponse(result, "Create process task successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProcessTaskResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id,[FromBody] ProcessTaskRequestDto request)
        {
            var result = await _processTaskService.UpdateProcessTaskByIdAsync(id, request);
            return Ok(ApiResponse<ProcessTaskResponseDto>.SuccessResponse(result, "Update process task successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProcessTaskResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _processTaskService.DeleteProcessTaskByIdAsync(id);
            return Ok(ApiResponse<ProcessTaskResponseDto>.SuccessResponse(result, "Delete process task successfully"));
        }

        [HttpPatch("complete-processTask-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ProcessTaskResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CompleteProcessTaskAsync(Guid id)
        {
            var result = await _processTaskService.CompleteProcessTaskAsync(id);
            return Ok(ApiResponse<ProcessTaskResponseDto>.SuccessResponse(result, "ProcessTask has completed"));
        }
    }
}
