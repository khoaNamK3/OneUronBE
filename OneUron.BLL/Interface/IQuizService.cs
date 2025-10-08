using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IQuizService
    {
        public  Task<ApiResponse<List<QuizResponseDto>>> GetAllQuizAsync();

        public  Task<ApiResponse<QuizResponseDto>> GetQuizByIdAsync(Guid id);

        public  Task<ApiResponse<QuizResponseDto>> CreateNewQuizAsync(QuizRequestDto reuqest);

        public  Task<ApiResponse<QuizResponseDto>> UpdateQuizByIdAsync(Guid id, QuizRequestDto newQuiz);

        public  Task<ApiResponse<QuizResponseDto>> DeleteQuizByIdAsync(Guid id);

        public QuizResponseDto MapToDTO(Quiz quiz);

        public Quiz MaptoEntity(QuizRequestDto newQuiz);

    }
}
