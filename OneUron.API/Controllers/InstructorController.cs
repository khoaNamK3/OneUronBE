using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.InstructorDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController : ControllerBase
    {
        private readonly IInstructorService _instructorService;

        public InstructorController(IInstructorService instructorService)
        {
            _instructorService = instructorService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _instructorService.GetAllAsync();
            return Ok(ApiResponse<List<InstructorResponseDto>>.SuccessResponse(result, "Get all instructors successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _instructorService.GetInstructorByIdAsync(id);
            return Ok(ApiResponse<InstructorResponseDto>.SuccessResponse(result, "Get instructor by ID successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(InstructorRequestDto request)
        {
            var result = await _instructorService.CreateNewInstructorAsync(request);
            return Ok(ApiResponse<InstructorResponseDto>.SuccessResponse(result, "Create instructor successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, InstructorRequestDto request)
        {
            var result = await _instructorService.UpdateInstructorByIdAsync(id, request);
            return Ok(ApiResponse<InstructorResponseDto>.SuccessResponse(result, "Update instructor successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _instructorService.DeleteInstructorByIdAsync(id);
            return Ok(ApiResponse<InstructorResponseDto>.SuccessResponse(result, "Delete instructor successfully"));
        }
    }
}
