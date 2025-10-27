using OneUron.BLL.DTOs.EvaluationDTOs;
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
    public interface IEvaluationService 
    {
        Task<List<EvaluationResponseDto>> GetAllAsync();
        Task<EvaluationResponseDto> GetByIdAsync(Guid id);
        Task<EvaluationResponseDto> CreateNewEvaluationAsync(EvaluationRequestDto request);
        Task<EvaluationResponseDto> UpdateEvaluationByIdAsync(Guid id, EvaluationRequestDto newEvaluation);
        Task<EvaluationResponseDto> DeleteEvaluationByIdAsync(Guid id);

        public EvaluationResponseDto MapToDTO(Evaluation evaluation);

        public  Task<PagedResult<EvaluationPagingResponse>> GetAllPaging(int pageNumber, int pageSize, string? name);
    }
}
