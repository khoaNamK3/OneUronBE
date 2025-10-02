using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MethodRuleController : Controller
    {
        private readonly IMethodRuleService _methodRuleService;

        public MethodRuleController(IMethodRuleService methodRuleService)
        {
            _methodRuleService = methodRuleService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _methodRuleService.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetMethodRuleByIdAsyn(Guid id)
        {
            var response = await _methodRuleService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("create-new")]
        public async Task<IActionResult> CreateNewMethodRuleAsync([FromBody] MethodRuleRequestDto request)
        {
            var response = await _methodRuleService.CreateNewMethodRuleAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateMethodRuleByIdAsync(Guid id, [FromBody] MethodRuleRequestDto request)
        {
            var response = await _methodRuleService.UpdateMethodRuleByIdAsync(id, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteMethodRuleByIdAsync(Guid id)
        {
            var response = await _methodRuleService.DeleteMethodRuleByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
