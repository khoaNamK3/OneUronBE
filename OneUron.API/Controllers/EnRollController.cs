using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.EnRollDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnRollController : ControllerBase
    {
        private readonly IEnRollService _enRollService;

        public EnRollController(IEnRollService enRollService)
        {
            _enRollService = enRollService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<EnRollResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _enRollService.GetAllEnRollAsync();
            return Ok(ApiResponse<List<EnRollResponseDto>>.SuccessResponse(result, "Get all enrollments successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<EnRollResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _enRollService.GetEnRollByIdAsync(id);
            return Ok(ApiResponse<EnRollResponseDto>.SuccessResponse(result, "Get enrollment by ID successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<EnRollResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] EnRollRequestDto request)
        {
            var result = await _enRollService.CreateNewEnRollAsync(request);
            return Ok(ApiResponse<EnRollResponseDto>.SuccessResponse(result, "Create enrollment successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<EnRollResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] EnRollRequestDto request)
        {
            var result = await _enRollService.UpdateEnRollByIdAsync(id, request);
            return Ok(ApiResponse<EnRollResponseDto>.SuccessResponse(result, "Update enrollment successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<EnRollResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _enRollService.DeleteEnRollByIdAsync(id);
            return Ok(ApiResponse<EnRollResponseDto>.SuccessResponse(result, "Delete enrollment successfully"));
        }
    }
}