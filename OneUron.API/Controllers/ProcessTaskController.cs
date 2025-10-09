using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessTaskController : Controller
    {
        private readonly IProcessTaskService _processTaskService;

        public ProcessTaskController(IProcessTaskService processTaskService)
        {
            _processTaskService = processTaskService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _processTaskService.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var response = await _processTaskService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateProcessTaskAsync([FromBody] ProcessTaskRequestDto request)
        {
            var response = await _processTaskService.CreateProcessTaskAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateProcessTaskByIdAsync(Guid id, [FromBody] ProcessTaskRequestDto request)
        {
            var response = await _processTaskService.UpdateProcessTaskByIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteProcessTaskbyIdAsync(Guid id)
        {
            var response = await _processTaskService.DeleteProcessTaskByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

    }
}
