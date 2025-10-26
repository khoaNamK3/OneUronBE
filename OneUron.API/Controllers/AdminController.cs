using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs.AdminDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;

namespace OneUron.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("get-admin-infor")]
        [ProducesResponseType(typeof(ApiResponse<AdminResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAdminInformationAsync()
        {
            var result = await _adminService.GetAdminInforAsync();
            return Ok(ApiResponse<AdminResponseDto>.SuccessResponse(result, "Get  successful  successfully"));
        }
    }
}
