using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.ChoiceDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChoiceController : Controller
    {
        private readonly IChoiceService _choiceService;

        public ChoiceController(IChoiceService choiceService)
        {
            _choiceService = choiceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _choiceService.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetChoiceByIdAsync(Guid id)
        {
            var response = await _choiceService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewChoiceAsync([FromBody] ChoiceRequestDto request)
        {
            var response = await _choiceService.CreateNewChoiceAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateChoiceByIdAsync(Guid id, [FromBody] ChoiceRequestDto request)
        {
            var response = await _choiceService.UpdateChoiceByIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChoiceByIdAsync(Guid id)
        {
            var response = await _choiceService.DeleteChoiceByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
