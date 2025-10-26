using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.DTOs.SkillDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillController : ControllerBase
    {
        private readonly ISkillService _skillService;

        public SkillController(ISkillService skillService)
        {
            _skillService = skillService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<SkillResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _skillService.GetAllAsync();
            return Ok(ApiResponse<List<SkillResponseDto>>.SuccessResponse(result, "Get all skills successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<SkillResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _skillService.GetByIdAsync(id);
            return Ok(ApiResponse<SkillResponseDto>.SuccessResponse(result, "Get skill successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<SkillResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] SkillRequestDto request)
        {
            var result = await _skillService.CreateNewSkillAsync(request);
            return Ok(ApiResponse<SkillResponseDto>.SuccessResponse(result, "Create skill successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<SkillResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] SkillRequestDto request)
        {
            var result = await _skillService.UpdateSkillByIdAsync(id, request);
            return Ok(ApiResponse<SkillResponseDto>.SuccessResponse(result, "Update skill successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<SkillResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _skillService.DeleteSkillByIdAsync(id);
            return Ok(ApiResponse<SkillResponseDto>.SuccessResponse(result, "Delete skill successfully"));
        }
    }
}
