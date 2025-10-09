using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.SubjectDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : Controller
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _subjectService.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var response = await _subjectService.GetByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateSubjectAsync([FromBody] SubjectRequestDto request)
        {
            var response = await _subjectService.CreateNewSubjectAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateSubjectByIdAsync(Guid id, [FromBody] SubjectRequestDto request)
        {
            var response = await _subjectService.UpdateSubjectByIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }


        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteSubjectByIdAsync(Guid id)
        {
            var response = await _subjectService.DeleteSubjectByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
