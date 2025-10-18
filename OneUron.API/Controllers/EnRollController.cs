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
        public async Task<IActionResult> GetAll()
        {
            var result = await _enRollService.GetAllEnRollAsync();
            return Ok(ApiResponse<List<EnRollResponseDto>>.SuccessResponse(result, "Get all enrollments successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _enRollService.GetEnRollByIdAsync(id);
            return Ok(ApiResponse<EnRollResponseDto>.SuccessResponse(result, "Get enrollment by ID successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(EnRollRequestDto request)
        {
            var result = await _enRollService.CreateNewEnRollAsync(request);
            return Ok(ApiResponse<EnRollResponseDto>.SuccessResponse(result, "Create enrollment successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, EnRollRequestDto request)
        {
            var result = await _enRollService.UpdateEnRollByIdAsync(id, request);
            return Ok(ApiResponse<EnRollResponseDto>.SuccessResponse(result, "Update enrollment successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _enRollService.DeleteEnRollByIdAsync(id);
            return Ok(ApiResponse<EnRollResponseDto>.SuccessResponse(result, "Delete enrollment successfully"));
        }
    }
}