using OneUron.BLL.Services;
using OneUron.BLL.DTOs;
using OneUron.BLL.DTOs.AuthDTOs;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.UserRepo;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

namespace OneUron.BLL.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public AuthService(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userRepository.GetByUserNameAndPasswordAsync(request.UserName, request.Password);
            if (user == null)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            // Generate and store tokens
            var (accessToken, refreshToken) = await _jwtService.SaveTokensAsync(user.Id);

            return new LoginResponseDto
            {
                Success = true,
                Message = "Login successful",
                UserId = user.Id,
                Token = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            // Kiểm tra username đã tồn tại chưa
            var existingUser = await _userRepository.FindAsync(u => u.UserName == request.UserName && !u.IsDeleted);
            if (existingUser.Any())
            {
                return new RegisterResponseDto
                {
                    Success = false,
                    Message = "Username already exists"
                };
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.UserName,
                Password = request.Password, // Nên mã hóa password!
                CreatedDate = DateTime.UtcNow,
                UpdateDate = DateTime.UtcNow,
                IsDeleted = false
            };

            await _userRepository.AddAsync(user);

            return new RegisterResponseDto
            {
                Success = true,
                Message = "Register successful",
                UserId = user.Id
            };
        }

        public async Task<LogoutResponseDto> LogoutAsync(Guid userId)
        {
            await _jwtService.RevokeRefreshTokenAsync(userId);

            return new LogoutResponseDto
            {
                Success = true,
                Message = "Logged out successfully"
            };
        }

        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            try
            {
                // Validate the expired access token
                var principal = _jwtService.GetPrincipalFromExpiredToken(request.AccessToken);
                var userId = Guid.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                
                // Refresh the token
                var (success, newAccessToken, newRefreshToken) = await _jwtService.RefreshTokenAsync(request.RefreshToken);
                
                if (!success)
                {
                    return new RefreshTokenResponseDto
                    {
                        Success = false,
                        Message = "Invalid refresh token"
                    };
                }
                
                return new RefreshTokenResponseDto
                {
                    Success = true,
                    Message = "Token refreshed successfully",
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };
            }
            catch (Exception ex)
            {
                return new RefreshTokenResponseDto
                {
                    Success = false,
                    Message = "Error refreshing token: " + ex.Message
                };
            }
        }
    }
}
