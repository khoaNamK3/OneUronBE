﻿using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.AcknowledgeDTOs;
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
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _acknowledgeService.GetAllAcknowledgeAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("get-by/{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var response = await _acknowledgeService.GetAcknowledgeByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost("create-new")]
        public async Task<IActionResult> CreateNewAcknowLedgeAsync([FromBody] AcknowledgeRequestDto request)
        {
            var response = await _acknowledgeService.CreateNewAcknowledgeAsync(request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("update-by/{id}")]
        public async Task<IActionResult> UpdateAcknowLedgeByIdAsync(Guid id, [FromBody] AcknowledgeRequestDto request)
        {
            var response = await _acknowledgeService.UpdateAcknowLedgeByIdAsync(id, request);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpDelete("delete-by/{id}")]
        public async Task<IActionResult> DeleteAcknowLedgeByIdAsync(Guid id)
        {
            var response = await _acknowledgeService.DeleteAcknowledgeByIdAsync(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
