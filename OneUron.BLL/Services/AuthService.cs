using OneUron.BLL.Services;
using OneUron.BLL.DTOs;
using OneUron.BLL.DTOs.AuthDTOs;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.UserRepo;
using OneUron.DAL.Repository.RoleRepo;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using BCrypt.Net;

namespace OneUron.BLL.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;
        private readonly IRoleRepository _roleRepository;

        public AuthService(IUserRepository userRepository, JwtService jwtService, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _roleRepository = roleRepository;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            // Get user by username with roles included
            var user = await _userRepository.GetByUserNameWithRolesAsync(request.UserName);
            if (user == null)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid username or password"
                };
            }

            // Verify password hash (assuming you're using BCrypt for hashing)
            bool passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!passwordValid)
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
                RefreshToken = refreshToken,
                Roles = user.Roles?.Select(r => r.RoleName).ToList() // Include roles in response
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            try
            {
                // Check if username already exists
                var existingUser = await _userRepository.FindAsync(u => u.UserName == request.UserName && !u.IsDeleted);
                if (existingUser.Any())
                {
                    return new RegisterResponseDto
                    {
                        Success = false,
                        Message = "Username already exists"
                    };
                }

                // Hash password
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                // Create new user
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = request.UserName,
                    Password = passwordHash,
                    CreatedDate = DateTime.UtcNow,
                    UpdateDate = DateTime.UtcNow,
                    IsDeleted = false,
                    Roles = new List<DAL.Data.Entity.Role>()
                };

                await _userRepository.AddAsync(user);

                // Assign default role or specified role
                string roleName = !string.IsNullOrEmpty(request.Role) ? request.Role : "User";
                await _userRepository.AssignRoleToUserAsync(user.Id, roleName);

                return new RegisterResponseDto
                {
                    Success = true,
                    Message = "Register successful",
                    UserId = user.Id
                };
            }
            catch (Exception ex)
            {
                return new RegisterResponseDto
                {
                    Success = false,
                    Message = $"Registration failed: {ex.Message}"
                };
            }
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
