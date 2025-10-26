using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.CourseDetailDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseDetailController : ControllerBase
    {
        private readonly ICourseDetailService _courseDetailService;

        public CourseDetailController(ICourseDetailService courseDetailService)
        {
            _courseDetailService = courseDetailService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<CourseDetailResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _courseDetailService.GetAllCourseDetailAsync();
            return Ok(ApiResponse<List<CourseDetailResponseDto>>.SuccessResponse(result, "Get all course details successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<CourseDetailResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _courseDetailService.GetCourseDetailByIdAsync(id);
            return Ok(ApiResponse<CourseDetailResponseDto>.SuccessResponse(result, "Get course detail by id successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<CourseDetailResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] CourseDetailRequestDto request)
        {
            var result = await _courseDetailService.CreateNewCourseDetailAsync(request);
            return Ok(ApiResponse<CourseDetailResponseDto>.SuccessResponse(result, "Create course detail successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<CourseDetailResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] CourseDetailRequestDto request)
        {
            var result = await _courseDetailService.UpdateCourseDetailByIdAsync(id, request);
            return Ok(ApiResponse<CourseDetailResponseDto>.SuccessResponse(result, "Update course detail successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<CourseDetailResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _courseDetailService.DeleteCourseDetailByIdAsync(id);
            return Ok(ApiResponse<CourseDetailResponseDto>.SuccessResponse(result, "Delete course detail successfully"));
        }
    }
}
