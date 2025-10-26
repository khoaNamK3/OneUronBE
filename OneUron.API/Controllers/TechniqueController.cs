﻿using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.SubjectDTOs;
using OneUron.BLL.DTOs.TechniqueDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TechniqueController : ControllerBase
    {
        private readonly ITechniqueService _techniqueService;

        public TechniqueController(ITechniqueService techniqueService)
        {
            _techniqueService = techniqueService;
        }

        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<TechniqueResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _techniqueService.GetAllAsync();
            return Ok(ApiResponse<List<TechniqueResponseDto>>.SuccessResponse(result, "Get all techniques successfully"));
        }

        [HttpGet("get-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<TechniqueResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _techniqueService.GetByIdAsync(id);
            return Ok(ApiResponse<TechniqueResponseDto>.SuccessResponse(result, "Get technique successfully"));
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<TechniqueResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] TechniqueRequestDto request)
        {
            var result = await _techniqueService.CreateAsync(request);
            return Ok(ApiResponse<TechniqueResponseDto>.SuccessResponse(result, "Create technique successfully"));
        }

        [HttpPut("update-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<TechniqueResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id,[FromBody] TechniqueRequestDto request)
        {
            var result = await _techniqueService.UpdateByIdAsync(id, request);
            return Ok(ApiResponse<TechniqueResponseDto>.SuccessResponse(result, "Update technique successfully"));
        }

        [HttpDelete("delete-by/{id}")]
        [ProducesResponseType(typeof(ApiResponse<TechniqueResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _techniqueService.DeleteByIdAsync(id);
            return Ok(ApiResponse<TechniqueResponseDto>.SuccessResponse(result, "Delete technique successfully"));
        }
    }
}
