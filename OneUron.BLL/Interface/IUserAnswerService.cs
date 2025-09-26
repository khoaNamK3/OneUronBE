using OneUron.BLL.DTOs.UserAnswerDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IUserAnswerService
    {
        public Task<ApiResponse<List<UserAnswerResponseDto>>> GetAllAsync();
        public Task<ApiResponse<List<UserAnswerResponseDto>>> GetByListUserAnswerAsync(Guid userId, Guid eluationQuestionId);

        public Task<ApiResponse<UserAnswerResponseDto>> CreateNewUserAnswerAsync(UserAnswerRequestDto resquest);

        public  Task<ApiResponse<UserAnswerResponseDto>> UpdateUserAnswerByUserIdAsync(Guid id, UserAnswerUpdateRequestDto request);

        public  Task<ApiResponse<UserAnswerResponseDto>> DeleteUserAnswerByAsync(Guid id);

        public UserAnswerResponseDto MaptoDTO(UserAnswer userAnswer);
    }
}
