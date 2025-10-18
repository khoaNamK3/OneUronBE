using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.StudyMethodDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudyMethodController : ControllerBase
    {
        private readonly IStudyMethodService _studyMethodService;

        public StudyMethodController(IStudyMethodService studyMethodService)
        {
            _studyMethodService = studyMethodService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _studyMethodService.GetAllAsync();
            return Ok(ApiResponse<List<StudyMethodResponseDto>>.SuccessResponse(result, "Get all study methods successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _studyMethodService.GetByIdAsync(id);
            return Ok(ApiResponse<StudyMethodResponseDto>.SuccessResponse(result, "Get study method successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(StudyMethodRequestDto request)
        {
            var result = await _studyMethodService.CreateAsync(request);
            return Ok(ApiResponse<StudyMethodResponseDto>.SuccessResponse(result, "Create study method successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, StudyMethodRequestDto request)
        {
            var result = await _studyMethodService.UpdateByIdAsync(id, request);
            return Ok(ApiResponse<StudyMethodResponseDto>.SuccessResponse(result, "Update study method successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _studyMethodService.DeleteByIdAsync(id);
            return Ok(ApiResponse<StudyMethodResponseDto>.SuccessResponse(result, "Delete study method successfully"));
        }
    }
}
