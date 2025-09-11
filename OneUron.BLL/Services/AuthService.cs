using OneUron.BLL.DTOs;
using OneUron.BLL.DTOs.AuthDTOs;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.UserRepo;
using System;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

            // TODO: Generate JWT token here
            string token = "fake-jwt-token"; // Replace with real JWT generation

            return new LoginResponseDto
            {
                Success = true,
                Message = "Login successful",
                UserId = user.Id,
                Token = token
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
    }

}
