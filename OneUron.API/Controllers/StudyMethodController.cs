using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.SkillDTOs;
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
        [ProducesResponseType(typeof(ApiResponse<List<StudyMethodResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _studyMethodService.GetAllAsync();
            return Ok(ApiResponse<List<StudyMethodResponseDto>>.SuccessResponse(result, "Get all study methods successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<StudyMethodResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _studyMethodService.GetByIdAsync(id);
            return Ok(ApiResponse<StudyMethodResponseDto>.SuccessResponse(result, "Get study method successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<StudyMethodResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] StudyMethodRequestDto request)
        {
            var result = await _studyMethodService.CreateAsync(request);
            return Ok(ApiResponse<StudyMethodResponseDto>.SuccessResponse(result, "Create study method successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<StudyMethodResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] StudyMethodRequestDto request)
        {
            var result = await _studyMethodService.UpdateByIdAsync(id, request);
            return Ok(ApiResponse<StudyMethodResponseDto>.SuccessResponse(result, "Update study method successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<StudyMethodResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _studyMethodService.DeleteByIdAsync(id);
            return Ok(ApiResponse<StudyMethodResponseDto>.SuccessResponse(result, "Delete study method successfully"));
        }

        [HttpGet("get-study-method-by/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<StudyMethodResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStudyMethodByUserIdAsync(Guid userId)
        {
            var result = await _studyMethodService.GetStudyMethodByUserIdAsync(userId);
            return Ok(ApiResponse<StudyMethodResponseDto>.SuccessResponse(result, "Get All studyMethod By UserId successfully"));
        }
    }
}
