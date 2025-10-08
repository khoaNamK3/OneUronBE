using OneUron.BLL.DTOs.UserQuizAttemptDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IUserQuizAttemptService
    {
        public Task<ApiResponse<List<UserQuizAttemptResponseDto>>> GetAllUserQuizAttemptAsync();

        public Task<ApiResponse<UserQuizAttemptResponseDto>> GetUserQuizAttemptsByIdAsync(Guid id);

        public  Task<ApiResponse<UserQuizAttemptResponseDto>> CreateNewUserQuizAttemptAsync(UserQuizAttemptRequestDto request);

        public  Task<ApiResponse<UserQuizAttemptResponseDto>> UpdateUserQuizAttemptByIdAsync(Guid id, UserQuizAttemptRequestDto newUserQuizAttempt);

        public  Task<ApiResponse<UserQuizAttemptResponseDto>> DeleteUserQuizAttemptByIdAsync(Guid id);

        public UserQuizAttemptResponseDto MapToDTO(UserQuizAttempt userQuizAttempt);

    }
}
