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
        public Task<ApiResponse<List<QuestionResponseDto>>> GetAllAsync();

        public Task<ApiResponse<QuestionResponseDto>> GetbyIdAsync(Guid id);

        public  Task<ApiResponse<QuestionResponseDto>> CreateNewQuestionAsync(QuestionRequestDto request);

        public  Task<ApiResponse<QuestionResponseDto>> UpdateQuestionByIdAsync(Guid id, QuestionRequestDto newQuestion);

        public  Task<ApiResponse<QuestionResponseDto>> DeleteQuestionByIdAsync(Guid id);

        public QuestionResponseDto MapToDTO(Question question);

    }
}
