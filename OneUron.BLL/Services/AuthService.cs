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
                    Message = "Sai tài khoản hoặc mật khẩu"
                };
            }

            // Verify password hash (assuming you're using BCrypt for hashing)
            bool passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
            if (!passwordValid)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "Sai tài khoản hoặc mật khẩu"
                };
            }

           if(user.IsDeleted == true)
            {
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "tài khoản của bản đã bị xóa , hãy đăng kí lại nhé"
                };
            }

            // Generate and store tokens
            var (accessToken, refreshToken) = await _jwtService.SaveTokensAsync(user.Id);

            return new LoginResponseDto
            {
                Success = true,
                Message = "Đăng nhập thành công",
                UserId = user.Id,
                Token = accessToken,
                RefreshToken = refreshToken,
                Roles = user.Roles?.Select(r => r.RoleName).ToList() // Include roles in response
            };
        }

        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            // Sử dụng transaction để đảm bảo tính nhất quán của dữ liệu
            using (var transaction = await _userRepository.BeginTransactionAsync())
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
                            Message = "Tên người dùng đã tồn tại."
                        };
                    }

                    // Hash password
                    string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

                    // Create new profile
                    var profile = new Profile
                    {
                        Id = Guid.NewGuid(),
                        FullName = request.FullName,
                        Address = request.Address,
                        Avatar = string.Empty,
                        Dob = request.Dob
                    };

                    // Create new user with profile
                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        UserName = request.UserName,
                        Password = passwordHash,
                        CreatedDate = DateTime.UtcNow,
                        UpdateDate = DateTime.UtcNow,
                        IsDeleted = false,
                        Profile = profile,
                        Roles = new List<Role>()
                    };

                    // Liên kết ngược từ profile đến user
                    profile.UserId = user.Id;

                    // Thêm user vào database
                    await _userRepository.AddAsync(user);

                    // Kiểm tra role tồn tại trước khi thêm
                    string roleName = !string.IsNullOrEmpty(request.Role) ? request.Role : "User";

                    // Kiểm tra xem role có tồn tại không
                    var role = await _roleRepository.GetByNameAsync(roleName);
                    if (role == null)
                    {
                        // Nếu role không tồn tại, hủy giao dịch và trả về thông báo lỗi
                        await transaction.RollbackAsync();
                        return new RegisterResponseDto
                        {
                            Success = false,
                            Message = $"Quyền hạn '{roleName}' không tồn tại"
                        };
                    }

                    // Gán role cho user
                    await _userRepository.AssignRoleToUserAsync(user.Id, roleName);

                    // Hoàn thành giao dịch nếu mọi thứ thành công
                    await transaction.CommitAsync();

                    return new RegisterResponseDto
                    {
                        Success = true,
                        Message = "Đăng kí thành công",
                        UserId = user.Id
                    };
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi, hủy giao dịch
                    await transaction.RollbackAsync();
                    return new RegisterResponseDto
                    {
                        Success = false,
                        Message = $"Đăng kí thất bại: {ex.Message}"
                    };
                }
            }
        }

        public async Task<LogoutResponseDto> LogoutAsync(Guid userId)
        {
            await _jwtService.RevokeRefreshTokenAsync(userId);

            return new LogoutResponseDto
            {
                Success = true,
                Message = "Đăng xuất thành công"
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
