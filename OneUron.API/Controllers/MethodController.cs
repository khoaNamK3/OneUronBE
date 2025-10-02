using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.MethodDTOs;
using OneUron.BLL.ExceptionHandle;
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

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _methodSerivce.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id:guid}")]
        public async Task<IActionResult> GetMethodByIdAsync(Guid id)
        {
            var response = await _methodSerivce.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("create-new")]
        public async Task<IActionResult> CreateNewMethodAsync([FromBody] MethodRequestDto request)
        {
            var response = await _methodSerivce.CreateNewMethodAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateMethodByIdAsync(Guid id, MethodRequestDto request)
        {
            var response = await _methodSerivce.UpdateMethodByIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteMethodByIdAsync(Guid id)
        {
            var response = await _methodSerivce.DeleteMethodByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-top3/{userId:guid}")]
        public async Task<IActionResult> GetTop3MetodForUserAsync(Guid userId)
        {
            var response = await _methodSerivce.GetTop3MetodForUserAsync(userId);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }
}
