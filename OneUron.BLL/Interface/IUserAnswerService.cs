using OneUron.BLL.DTOs.EvaluationDTOs;
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
        Task<List<UserAnswerResponseDto>> GetAllAsync();
        Task<List<UserAnswerResponseDto>> GetByListAsync(Guid userId, Guid evaluationQuestionId);
        Task<UserAnswerResponseDto> CreateAsync(UserAnswerRequestDto request);
        Task<UserAnswerResponseDto> UpdateByIdAsync(Guid id, UserAnswerUpdateRequestDto request);
        Task<UserAnswerResponseDto> DeleteByIdAsync(Guid id);
        Task<List<UserAnswerResponseDto>> SubmitAnswersAsync(List<EvaluationSubmitRequest> evaluations);
        Task<List<UserAnswerResponseDto>> GetAllByUserIdAsync(Guid userId);
        UserAnswerResponseDto MapToDTO(UserAnswer userAnswer);
    }
}
