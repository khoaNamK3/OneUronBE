using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.EvaluationQuestionDTOs;
using OneUron.BLL.DTOs.FeatureDTOs;
using OneUron.BLL.DTOs.PaymentDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeatureController : Controller
    {
      
        private readonly IfeatureService _featureService;
        public FeatureController(IfeatureService featureService)
        {
            _featureService = featureService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<FeatureResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _featureService.GetAllAsync();

            return Ok(ApiResponse<List<FeatureResponseDto>>.SuccessResponse(response, "Get All Feature Successfully"));
        }

        [HttpGet("get-by/{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<FeatureResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var resposne = await _featureService.GetByIdAsync(id);
            return Ok(ApiResponse<FeatureResponseDto>.SuccessResponse(resposne, "Get Feature By Id Successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<FeatureResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateNewFeatureAsync([FromBody] FeatureRequestDto requestDto)
        {
            var response = await _featureService.CreateFeatureAsync(requestDto);

            return Ok(ApiResponse<FeatureResponseDto>.SuccessResponse(response, "Create new Feature Sucessfully"));
        }


        [HttpPut("update-by/{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<FeatureResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateFeatureByIdAsync(Guid id, [FromBody]FeatureRequestDto requestDto)
        {
            var response = await _featureService.UpdateFeatureByIdAsync(id, requestDto);

            return Ok(ApiResponse<FeatureResponseDto>.SuccessResponse(response, "Update Feature Successfully"));
        }

        [HttpDelete("delete-by/{id:guid}")]
        [ProducesResponseType(typeof(ApiResponse<FeatureResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteFeatureByIdAsync(Guid id)
        {
            var response = await _featureService.DeleteFuatureByIdAsync(id);

            return Ok(ApiResponse<FeatureResponseDto>.SuccessResponse(response, "Delete Feature Successfully"));
        }
    }
}
