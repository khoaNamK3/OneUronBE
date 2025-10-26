using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.MemberShipPlanDTOs;
using OneUron.BLL.DTOs.MethodConDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MethodConController : ControllerBase
    {
        private readonly IMethodConService _methodConService;

        public MethodConController(IMethodConService methodConService)
        {
            _methodConService = methodConService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<MethodConResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _methodConService.GetAllAsync();
            return Ok(ApiResponse<List<MethodConResponseDto>>.SuccessResponse(result, "Get all MethodCon successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MethodConResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _methodConService.GetByIdAsync(id);
            return Ok(ApiResponse<MethodConResponseDto>.SuccessResponse(result, "Get MethodCon by id successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<MethodConResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] MethodConRequestDto request)
        {
            var result = await _methodConService.CreateNewMethodConAsync(request);
            return Ok(ApiResponse<MethodConResponseDto>.SuccessResponse(result, "Create MethodCon successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MethodConResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id,[FromBody] MethodConRequestDto request)
        {
            var result = await _methodConService.UpdateMethodConByIdAsync(id, request);
            return Ok(ApiResponse<MethodConResponseDto>.SuccessResponse(result, "Update MethodCon successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MethodConResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _methodConService.DeleteMethodConByIdAsync(id);
            return Ok(ApiResponse<MethodConResponseDto>.SuccessResponse(result, "Delete MethodCon successfully"));
        }
    }
}
