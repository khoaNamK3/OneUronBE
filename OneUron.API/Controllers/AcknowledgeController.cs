using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.AcknowledgeDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AcknowledgeController : Controller
    {
        private readonly IAcknowledgeService _acknowledgeService;

        public AcknowledgeController(IAcknowledgeService acknowledgeService)
        {
            _acknowledgeService = acknowledgeService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<AcknowledgeResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _acknowledgeService.GetAllAcknowledgeAsync();
            return Ok(ApiResponse<List<AcknowledgeResponseDto>>.SuccessResponse(result, "Get all acknowledges successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<AcknowledgeResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _acknowledgeService.GetAcknowledgeByIdAsync(id);
            return Ok(ApiResponse<AcknowledgeResponseDto>.SuccessResponse(result, "Get acknowledge by id successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<AcknowledgeResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] AcknowledgeRequestDto request)
        {
            var result = await _acknowledgeService.CreateNewAcknowledgeAsync(request);
            return Ok(ApiResponse<AcknowledgeResponseDto>.SuccessResponse(result, "Create acknowledge successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<AcknowledgeResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, [FromBody] AcknowledgeRequestDto request)
        {
            var result = await _acknowledgeService.UpdateAcknowLedgeByIdAsync(id, request);
            return Ok(ApiResponse<AcknowledgeResponseDto>.SuccessResponse(result, "Update acknowledge successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<AcknowledgeResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _acknowledgeService.DeleteAcknowledgeByIdAsync(id);
            return Ok(ApiResponse<AcknowledgeResponseDto>.SuccessResponse(result, "Delete acknowledge successfully"));
        }
    }
}
