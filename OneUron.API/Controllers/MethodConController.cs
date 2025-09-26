using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.MethodConDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MethodConController : Controller
    {
        private readonly IMethodConService _MethodConService;

        public MethodConController(IMethodConService methodConService)
        {
            _MethodConService = methodConService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _MethodConService.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMethodConByIdAsync(Guid id)
        {
            var respone = await _MethodConService.GetByIdAsync(id);
            if (!respone.Success)
            {
                return NotFound(respone);
            }
            return Ok(respone);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewMethodConAsync([FromBody] MethodConRequestDto request)
        {
            var response = await _MethodConService.CreateNewMethodConAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMethodConbyIdAsyc(Guid id, [FromBody] MethodConRequestDto request)
        {
            var response = await _MethodConService.UpdateMethodConByIdAsync(id, request);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMethodConByIdAsync(Guid id)
        {
            var response = await _MethodConService.DeleteMethodConByIdAsync(id);

            if (!response.Success)
            {
               return NotFound(response);
            }
            return Ok(response);
        }
    }
}
