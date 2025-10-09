using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProcessController : Controller
    {
        private readonly IProcessService _processService;

        public ProcessController(IProcessService processService)
        {
            _processService = processService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _processService.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var respone = await _processService.GetByIdAsync(id);

            if (!respone.Success)
            {
                return NotFound(respone);
            }
            return Ok(respone);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateNewProcessAsync([FromBody] ProcessRequestDto request)
        {
            var response = await _processService.CreateProcessAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateProcessByIdAsync(Guid id, [FromBody] ProcessRequestDto request)
        {
            var response = await _processService.UpdateProcessByIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteProcessByIdAsync(Guid id)
        {
            var response = await _processService.DeleteProcessByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
