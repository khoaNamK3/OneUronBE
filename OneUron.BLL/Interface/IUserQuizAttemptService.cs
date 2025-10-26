using OneUron.BLL.DTOs.UserQuizAttemptDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IUserQuizAttemptService
    {
        Task<List<UserQuizAttemptResponseDto>> GetAllAsync();
        Task<UserQuizAttemptResponseDto> GetByIdAsync(Guid id);
        Task<UserQuizAttemptResponseDto> CreateAsync(UserQuizAttemptRequestDto request);
        Task<UserQuizAttemptResponseDto> UpdateByIdAsync(Guid id, UserQuizAttemptRequestDto request);
        Task<UserQuizAttemptResponseDto> DeleteByIdAsync(Guid id);
        Task<UserQuizAttemptResponseDto> MapToDTO(UserQuizAttempt attempt);

        Task<List<UserQuizAttemptResponseDto>> GetAllUserQuizAttemptByQuizIdAsync(Guid quizId);

        Task<UserQuizAttemptResponseDto> SubmitAnswerAsync(SubmitAnswerRequest newSubmit);

        public  Task<PagedResult<UserQuizAttemptResponseDto>> GetAllUserQuizAttempByUserIdAsync(int pageNumber, int pageSize, Guid userId);
    }
}
