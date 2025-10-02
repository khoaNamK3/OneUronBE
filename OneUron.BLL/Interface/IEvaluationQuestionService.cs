
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
        public Task<ApiResponse<List<EvaluationQuestionResponseDto>>> GetAllAsync();

        public Task<ApiResponse<EvaluationQuestionResponseDto>> GetEvaluationByIdAsync(Guid id);

        public  Task<ApiResponse<EvaluationQuestionResponseDto>> CreateNewEvaluationQuestionAsync(EvaluationQuestionRequestDto request);

        public  Task<ApiResponse<EvaluationQuestionResponseDto>> UpdateEvaluationQuestionByIdAsync(Guid id, EvaluationQuestionRequestDto newEvaluation);

        public  Task<ApiResponse<EvaluationQuestionResponseDto>> DeleteEvaluationQuestionByIdAsync(Guid id);

        public EvaluationQuestionResponseDto MapToDTO(EvaluationQuestion evaluationQuestion);

    }
}
