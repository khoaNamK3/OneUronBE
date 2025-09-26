using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.MethodDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MethodController : Controller
    {
        private readonly IMethodSerivce _methodSerivce;

        public MethodController(IMethodSerivce methodSerivce)
        {
            _methodSerivce = methodSerivce;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _methodSerivce.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMethodByIdAsync(Guid id)
        {
            var response = await _methodSerivce.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewMethodAsync([FromBody] MethodRequestDto request)
        {
            var response = await _methodSerivce.CreateNewMethodAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMethodByIdAsync(Guid id, MethodRequestDto request)
        {
            var response = await _methodSerivce.UpdateMethodByIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMethodByIdAsync(Guid id)
        {
            var response = await _methodSerivce.DeleteMethodByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
