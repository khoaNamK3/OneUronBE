using OneUron.BLL.DTOs.QuestionChoiceDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IQuestionChoiceService
    {
        public Task<ApiResponse<List<QuestionChoiceReponseDto>>> GetAllQuestionChoiceAsync();

        public Task<ApiResponse<QuestionChoiceReponseDto>> GetQuestionChoiceByIdAsync(Guid id);

        public  Task<ApiResponse<QuestionChoiceReponseDto>> CreateNewQuestionChoiceAsync(QuestionChoiceRequestDto request);

        public  Task<ApiResponse<QuestionChoiceReponseDto>> UpdateQuestionChoiceByIdAsync(Guid id, QuestionChoiceRequestDto newQuestionChoice);

        public  Task<ApiResponse<QuestionChoiceReponseDto>> DeleteQuestionChoiceByIdAsync(Guid id);

        public QuestionChoiceReponseDto MapToDTO(QuestionChoice question);
    }
}
