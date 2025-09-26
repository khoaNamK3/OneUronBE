using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.EvaluationQuestionDTOs;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EvaluationQuestionController : Controller
    {
        private readonly IEvaluationQuestionService _evaluationQuestionService;

        public EvaluationQuestionController(IEvaluationQuestionService evaluationQuestionService)
        {
            _evaluationQuestionService = evaluationQuestionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var response = await _evaluationQuestionService.GetAllAsync();

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetEvaluationQuestionByIdAsync(Guid id)
        {
            var response = await _evaluationQuestionService.GetEvaluationByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNewEvaluationQuestionAsync([FromBody] EvaluationQuestionRequestDto request)
        {
            var response = await _evaluationQuestionService.CreateNewEvaluationQuestionAsync(request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvaluationQuestionByIdAsync(Guid id, [FromBody] EvaluationQuestionRequestDto request)
        {
            var response = await _evaluationQuestionService.UpdateEvaluationQuestionByIdAsync(id, request);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvaluationQuestionByIdAsync(Guid id)
        {
            var response = await _evaluationQuestionService.DeleteEvaluationQuestionByIdAsync(id);

            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
