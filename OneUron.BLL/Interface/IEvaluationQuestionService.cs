
using OneUron.BLL.DTOs.EvaluationQuestionDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IEvaluationQuestionService
    {
        Task<List<EvaluationQuestionResponseDto>> GetAllAsync();
        Task<EvaluationQuestionResponseDto> GetByIdAsync(Guid id);
        Task<EvaluationQuestionResponseDto> CreateNewEvaluationQuestionAsync(EvaluationQuestionRequestDto request);
        Task<EvaluationQuestionResponseDto> UpdateEvaluationQuestionByIdAsync(Guid id, EvaluationQuestionRequestDto newEvaluation);
        Task<EvaluationQuestionResponseDto> DeleteEvaluationQuestionByIdAsync(Guid id);

        public EvaluationQuestionResponseDto MapToDTO(EvaluationQuestion evaluationQuestion);

    }
}
