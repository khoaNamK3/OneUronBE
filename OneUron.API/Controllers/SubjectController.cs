using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.SubjectDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _subjectService.GetAllAsync();
            return Ok(ApiResponse<List<SubjectResponseDto>>.SuccessResponse(result, "Get all subjects successfully"));
        }

        [HttpGet("schedule/{scheduleId:guid}/subjects")]
        public async Task<IActionResult> GetAllSubjectByScheduleIdAsync(Guid scheduleId)
        {
            var result = await _subjectService.GetAllSubjectbyScheduleIdAsync(scheduleId);
            return Ok(ApiResponse<List<SubjectResponseDto>>.SuccessResponse(result, "Get all subjects successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _subjectService.GetByIdAsync(id);
            return Ok(ApiResponse<SubjectResponseDto>.SuccessResponse(result, "Get subject successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(SubjectRequestDto request)
        {
            var result = await _subjectService.CreateAsync(request);
            return Ok(ApiResponse<SubjectResponseDto>.SuccessResponse(result, "Create subject successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, SubjectRequestDto request)
        {
            var result = await _subjectService.UpdateByIdAsync(id, request);
            return Ok(ApiResponse<SubjectResponseDto>.SuccessResponse(result, "Update subject successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _subjectService.DeleteByIdAsync(id);
            return Ok(ApiResponse<SubjectResponseDto>.SuccessResponse(result, "Delete subject successfully"));
        }
    }
}
