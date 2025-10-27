using OneUron.BLL.DTOs.AnswerDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IAnswerService
    {
        Task<List<AnswerResponseDto>> GetAllAnswerAsync();
        Task<AnswerResponseDto> GetAnswerByIdAsync(Guid id);
        Task<AnswerResponseDto> CreateNewAnswerAsync(AnswerRequestDto answerRequest);
        Task<AnswerResponseDto> UpdateAnswerByIdAsync(Guid id, AnswerRequestDto newAnswer);
        Task<AnswerResponseDto> DeleteAnswerByIdAsync(Guid id);

        public AnswerResponseDto MapToDTO(Answer answer);

        public Task CreateAnswerListAsync(List<AnswerRequestDto> dtoList);

    }
}
