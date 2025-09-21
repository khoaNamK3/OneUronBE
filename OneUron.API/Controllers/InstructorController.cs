using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.InstructorDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : Controller
    {
        private readonly IInstructorService _instructorService;


        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _instructorService.GetAllAsync();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstructorByIdAsync(Guid id)
        {
            var response = await _instructorService.GetInstructorByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewInstructorAsync([FromBody] InstructorRequestDto request)
        {
            var response = await _instructorService.CreateNewInstructorAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInstructorByIdAsync(Guid id, [FromBody] InstructorRequestDto request)
        {
            var response = await _instructorService.UpdateInstructorByIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInstructorByIdAsync(Guid id)
        {
            var response = await _instructorService.DeleteInstructorByIdAsync(id);
            if (!response.Success)
            {
            return NotFound(response);
            }
            return Ok(response);
        }
    }
}
