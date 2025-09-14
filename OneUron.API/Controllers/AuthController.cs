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
    private readonly JwtService _jwtService;

    public AuthController(AuthService authService, JwtService jwtService)
    {
        _authService = authService;
        _jwtService = jwtService;
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
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        // Get the refresh token from the X-Refresh-Token header
        string refreshToken = Request.Headers["X-Refresh-Token"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(new { message = "Refresh token is required" });
        }

        // Get the expired access token from the Authorization header
        string authHeader = Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return Unauthorized(new { message = "Access token is required" });
        }

        string accessToken = authHeader.Substring("Bearer ".Length).Trim();

        // Create a refresh token request using the values from headers
        var request = new RefreshTokenRequestDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        var response = await _authService.RefreshTokenAsync(request);
        if (!response.Success)
            return Unauthorized(new { message = response.Message });

        return Ok(response);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        // Get the token from the Authorization header
        string authHeader = Request.Headers["Authorization"];
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        string accessToken = authHeader.Substring("Bearer ".Length).Trim();

        // Get user ID from token and logout
        var principal = _jwtService.GetPrincipalFromExpiredToken(accessToken);
        var userId = System.Guid.Parse(principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
        var response = await _authService.LogoutAsync(userId);

        if (!response.Success)
            return BadRequest(new { message = response.Message });

        return Ok(response);
    }
}