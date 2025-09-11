using OneUron.BLL.DTOs;
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
    }
}
