using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.CourseDetailDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseDetailController : Controller
    {
        private readonly ICourseDetailService _courseDetailService;

        public CourseDetailController(ICourseDetailService courseDetailService)
        {
            _courseDetailService = courseDetailService;
        }


        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _courseDetailService.GetAllCourseDetailAsync();
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var response = await _courseDetailService.GetCourseDetailbyIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("create-new")]
        public async Task<IActionResult> CreateNewCourseDetailAsync([FromBody] CourseDetailRequestDto request)
        {
            var response = await _courseDetailService.CreateNewCourseDetailAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateCourseDetailByIdAsync(Guid id, [FromBody] CourseDetailRequestDto request)
        {
            var response = await _courseDetailService.UpdateCourseDetailByIdAsync(id, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteCourseDetailByIdAsync(Guid id)
        {
            var response = await _courseDetailService.DeleteCourseDetailByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
