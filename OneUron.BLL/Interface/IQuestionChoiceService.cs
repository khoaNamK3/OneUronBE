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
        Task<List<QuestionChoiceReponseDto>> GetAllQuestionChoiceAsync();
        Task<QuestionChoiceReponseDto> GetQuestionChoiceByIdAsync(Guid id);
        Task<QuestionChoiceReponseDto> CreateNewQuestionChoiceAsync(QuestionChoiceRequestDto request);
        Task<QuestionChoiceReponseDto> UpdateQuestionChoiceByIdAsync(Guid id, QuestionChoiceRequestDto request);
        Task<QuestionChoiceReponseDto> DeleteQuestionChoiceByIdAsync(Guid id);
        QuestionChoiceReponseDto MapToDTO(QuestionChoice entity);
    }
}
