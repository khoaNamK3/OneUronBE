using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.EvaluationDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationController : Controller
    {
        private readonly IEvaluationService _evaluationService;

        public EvaluationController(IEvaluationService evaluationService)
        {
            _evaluationService = evaluationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _evaluationService.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvaluationByIdAsync(Guid id)
        {
            var respone = await _evaluationService.GetbyIdAsync(id);

            if (!respone.Success)
            {
                return NotFound(respone);
            }
            return Ok(respone);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewEvaluationAsync([FromBody] EvaluationRequestDto requestDto)
        {
            var response = await _evaluationService.CreateNewEvaluationAsync(requestDto);

            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvaluationByIdAsync(Guid id, [FromBody] EvaluationRequestDto request)
        {
            var respone = await _evaluationService.UpdateEvaluationbyIdAsync(id, request);
            if (!respone.Success)
            {
                return BadRequest(respone);
            }
            return Ok(respone);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluationByIdAsync(Guid id)
        {
            var respone = await _evaluationService.DeleteEvaluationByIdAsync(id);

            if (!respone.Success)
            {
                return NotFound(respone);
            }
            return Ok(respone);
        }
    }
}
