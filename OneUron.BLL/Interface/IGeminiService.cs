using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.ExceptionHandle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IGeminiService
    {
        public  Task<ApiResponse<QuizResponseDto>> GenerateQuestionByQuizIdAsync(QuizRequestDto newQuiz);
    }
}
