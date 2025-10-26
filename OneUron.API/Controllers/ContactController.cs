using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.ContactDTO;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }


        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse<List<ContactResponseDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _contactService.GetAllContactAsync();

            return Ok(ApiResponse<List<ContactResponseDto>>.SuccessResponse(result, "Get all contacts successfully"));
        }

        [HttpGet("get/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ContactResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _contactService.GetByIdAsync(id);
            return Ok(ApiResponse<ContactResponseDto>.SuccessResponse(result, "Get contact by ID successfully"));
        }


        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse<ContactResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create([FromBody] ContactRequestDto requestDto)
        {
            var result = await _contactService.CreateNewContactAsync(requestDto);
            return Ok(ApiResponse<ContactResponseDto>.SuccessResponse(result, "Create contact successfully"));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(typeof(ApiResponse<ContactResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _contactService.DeleteContactByIdAsync(id);
            return Ok(ApiResponse<ContactResponseDto>.SuccessResponse(result, "Delete contact By Id successfully"));
        }
    }
}
