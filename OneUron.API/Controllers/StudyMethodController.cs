using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.StudyMethodDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudyMethodController : Controller
    {
        private readonly IStudyMethodService _studyMethodService;

        public StudyMethodController(IStudyMethodService studyMethodService)
        {
            _studyMethodService = studyMethodService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _studyMethodService.GetALlAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetStudyMethodByIdAsync(Guid id)
        {
            var response = await _studyMethodService.GetByIdAsyc(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("create-new")]
        public async Task<IActionResult> CreateNewStudyMethodAsync([FromBody] StudyMethodRequestDto request)
        {
            var response = await _studyMethodService.CreateNewStudyMethodAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateStudyMethodByIdAsync(Guid id, [FromBody] StudyMethodRequestDto request)
        {
            var response = await _studyMethodService.UpdateStudyMethodbyIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteStudyMethodByIdAsync(Guid id)
        {
            var response = await _studyMethodService.DeleteStudyMethodbyIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

    }
}
