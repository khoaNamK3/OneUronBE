using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.MethodProDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MethodProController : Controller
    {
        private readonly IMethodProSerivce _methodProService;

        public MethodProController(IMethodProSerivce methodProSerivce)
        {
            _methodProService = methodProSerivce;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _methodProService.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetMethodProByIdAsync(Guid id)
        {
            var response = await _methodProService.GetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("create-new")]
        public async Task<IActionResult> CreateNewMethodProAsync([FromBody] MethodProRequestDto request)
        {
            var response = await _methodProService.CreateNewMethoProAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateMethodProByIdAsync(Guid id, [FromBody] MethodProRequestDto request)
        {
            var response = await _methodProService.UpdateMethodProByIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteMethodProByIdAsync(Guid id)
        {
            var response = await _methodProService.DeleteMethodProByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
