using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.InstructorDTOs;
using OneUron.BLL.DTOs.MemberShipDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MemberShipController : Controller
    {
        private readonly IMemberShipService _memberShipService;

        public MemberShipController(IMemberShipService memberShipService)
        {
            _memberShipService = memberShipService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<MemberShipResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var respone = await _memberShipService.GetAllAsync();

            return Ok(ApiResponse<List<MemberShipResponseDto>>.SuccessResponse(respone, "Get All Successfully"));
        }


        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MemberShipResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var response = await _memberShipService.GetByIdAsync(id);

            return Ok(ApiResponse<MemberShipResponseDto>.SuccessResponse(response, "Get By Id Successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<MemberShipResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNewMemberShipAsync([FromBody] MemberShipRequestDto requestDto)
        {
            var response = await _memberShipService.CreateMemberShipAsync(requestDto);

            return Ok(ApiResponse<MemberShipResponseDto>.SuccessResponse(response, "Create new Sucessfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MemberShipResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateMemberShipByIdAsync(Guid id, [FromBody] MemberShipRequestDto requestDto)
        {
            var response = await _memberShipService.UpdateMemberShipByIdAsync(id, requestDto);

            return Ok(ApiResponse<MemberShipResponseDto>.SuccessResponse(response, "Update MemberShip By Id Successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MemberShipResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteMemberShipByIdAsync(Guid id)
        {
            var response = await _memberShipService.DeleteMemberShipByIdAsync(id);

            return Ok(ApiResponse<MemberShipResponseDto>.SuccessResponse(response, "Delete MemberShip By Id Successfully"));
        }
    }
}
