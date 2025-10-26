using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.MemberShipDTOs;
using OneUron.BLL.DTOs.MemberShipPlanDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberShipPlanController : Controller
    {
        private readonly IMemberShipPlanService _memberShipPlanService;
        public MemberShipPlanController(IMemberShipPlanService memberShipPlanService)
        {
            _memberShipPlanService = memberShipPlanService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<MemberShipPlanResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _memberShipPlanService.GetAllMembertShipPlanAsync();

            return Ok(ApiResponse<List<MemberShipPlanResponseDto>>.SuccessResponse(response, "Get All MemberShipPlan Successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MemberShipPlanResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetMemberShipPlanByIdAsync(Guid id)
        {
            var response = await _memberShipPlanService.GetMemberShipPlanByIdAsync(id);

            return Ok(ApiResponse<MemberShipPlanResponseDto>.SuccessResponse(response,"Get MemberShipPlan By Id Successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<MemberShipPlanResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNewMemberShipPlanAsync([FromBody] MemberShipPlanRequestDto requestDto)
        {
            var response = await  _memberShipPlanService.CreateMemberShipPlanAsync(requestDto);

            return Ok(ApiResponse<MemberShipPlanResponseDto>.SuccessResponse(response, "Create MemberShipPlan Successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MemberShipPlanResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateMemberShipPlanByIdAsync(Guid id, [FromBody] MemberShipPlanRequestDto requestDto)
        {
            var response = await _memberShipPlanService.UpdateMemberShipPlanByIdAsync(id, requestDto);

            return Ok(ApiResponse<MemberShipPlanResponseDto>.SuccessResponse(response, "Update MemberShipPlan By Id Successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<MemberShipPlanResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteMemberShipByDAsync(Guid id)
        {
            var response = await _memberShipPlanService.DeleteMemberShipPlanByIdAsync(id);

            return Ok(ApiResponse<MemberShipPlanResponseDto>.SuccessResponse(response, "Delete MemberShipPlan By Id Successfully"));
        }
    }
}
