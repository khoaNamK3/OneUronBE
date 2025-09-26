using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MethodRuleCondtionController : Controller
    {
        private readonly IMethodRuleConditionService _methodRuleConditionService;

        public MethodRuleCondtionController(IMethodRuleConditionService methodRuleConditionService)
        {
            _methodRuleConditionService = methodRuleConditionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _methodRuleConditionService.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMethodRuleConditionByIdAsync(Guid id)
        {
            var response = await _methodRuleConditionService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewMethodRuleCondtionAsync([FromBody] MethodRuleConditionRequestDto request)
        {
            var response = await _methodRuleConditionService.CreateNewMethodRuleConditionAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMethodRuleConditionByIdAsync(Guid id, [FromBody] MethodRuleConditionRequestDto request)
        {
            var respone = await _methodRuleConditionService.UpdateMethodRuleConditionByIdAsync(id, request);

            if (!respone.Success)
            {
                return BadRequest(respone);
            }
            return Ok(respone);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMethodRuleConditionByIdAsync(Guid id)
        {
            var response = await _methodRuleConditionService.DeleteMethodRuleConditionByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}

