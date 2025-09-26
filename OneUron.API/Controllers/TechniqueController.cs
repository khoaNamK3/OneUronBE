using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.TechniqueDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TechniqueController : Controller
    {
        private readonly ITechniqueService _techniqueService;

        public TechniqueController(ITechniqueService techniqueService)
        {
            _techniqueService = techniqueService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _techniqueService.GetAllAsync();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTechniqueByIdAsync(Guid id)
        {
            var response = await _techniqueService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewTechniqueAsync([FromBody] TechniqueRequestDto request)
        {
            var response = await _techniqueService.CreateNewTechiqueAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTechniqueByIdAsync(Guid id, [FromBody] TechniqueRequestDto request)
        {
            var response = await _techniqueService.UpdateTechniqueByIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTechniquebyIdAsync(Guid id)
        {
            var response = await _techniqueService.DeleteTechniqueByidAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
