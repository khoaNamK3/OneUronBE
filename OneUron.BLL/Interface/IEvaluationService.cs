using OneUron.BLL.DTOs.EvaluationDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IEvaluationService 
    {
        public Task<ApiResponse<List<EvaluationResponseDto>>> GetAllAsync();

        public Task<ApiResponse<EvaluationResponseDto>> GetbyIdAsync(Guid id);

        public  Task<ApiResponse<EvaluationResponseDto>> CreateNewEvaluationAsync(EvaluationRequestDto request);

        public  Task<ApiResponse<EvaluationResponseDto>> UpdateEvaluationbyIdAsync(Guid id, EvaluationRequestDto newEvaluation);

        public  Task<ApiResponse<EvaluationResponseDto>> DeleteEvaluationByIdAsync(Guid id);

        public EvaluationResponseDto MapToDTO(Evaluation evaluation);

    }
}
