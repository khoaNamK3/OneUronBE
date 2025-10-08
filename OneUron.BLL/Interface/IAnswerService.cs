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
        public Task<ApiResponse<List<AnswerResponseDto>>> GetAllAnswerAsync();

        public Task<ApiResponse<AnswerResponseDto>> GetAnswerByIdAsyc(Guid id);

        public  Task<ApiResponse<AnswerResponseDto>> CreateNewAnswerAsync(AnswerRequestDto answerRequest);

        public  Task<ApiResponse<AnswerResponseDto>> UpdateAnswerByIdAsync(Guid id, AnswerRequestDto newAnswer);

        public  Task<ApiResponse<AnswerResponseDto>> DeleteAnswerByIdAsync(Guid id);

        public AnswerResponseDto MapToDTO(Answer answer);

    }
}
