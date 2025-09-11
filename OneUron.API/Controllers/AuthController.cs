using Microsoft.AspNetCore.Mvc;
using OneUron.BLL.DTOs;
using OneUron.BLL.DTOs.AuthDTOs;
using OneUron.BLL.Services;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);
        if (!response.Success)
            return Unauthorized(new { message = response.Message });

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var response = await _authService.RegisterAsync(request);
        if (!response.Success)
            return BadRequest(new { message = response.Message });

        return Ok(response);
    }
}