using OneUron.BLL.DTOs.QuestionDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IQuestionService
    {
        Task<List<QuestionResponseDto>> GetAllAsync();
        Task<QuestionResponseDto> GetByIdAsync(Guid id);
        Task<QuestionResponseDto> CreateNewQuestionAsync(QuestionRequestDto request);
        Task<QuestionResponseDto> UpdateQuestionByIdAsync(Guid id, QuestionRequestDto request);
        Task<QuestionResponseDto> DeleteQuestionByIdAsync(Guid id);
        QuestionResponseDto MapToDTO(Question question);

    }
}
