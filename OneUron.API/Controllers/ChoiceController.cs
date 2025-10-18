using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.ChoiceDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChoiceController : ControllerBase
    {
        private readonly IChoiceService _choiceService;

        public ChoiceController(IChoiceService choiceService)
        {
            _choiceService = choiceService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _choiceService.GetAllAsync();
            return Ok(ApiResponse<List<ChoiceResponseDto>>.SuccessResponse(result, "Get all choices successfully"));
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _choiceService.GetByIdAsync(id);
            return Ok(ApiResponse<ChoiceResponseDto>.SuccessResponse(result, "Get choice by id successfully"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(ChoiceRequestDto request)
        {
            var result = await _choiceService.CreateNewChoiceAsync(request);
            return Ok(ApiResponse<ChoiceResponseDto>.SuccessResponse(result, "Create choice successfully"));
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> Update(Guid id, ChoiceRequestDto request)
        {
            var result = await _choiceService.UpdateChoiceByIdAsync(id, request);
            return Ok(ApiResponse<ChoiceResponseDto>.SuccessResponse(result, "Update choice successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _choiceService.DeleteChoiceByIdAsync(id);
            return Ok(ApiResponse<ChoiceResponseDto>.SuccessResponse(result, "Delete choice successfully"));
        }
    }
}
