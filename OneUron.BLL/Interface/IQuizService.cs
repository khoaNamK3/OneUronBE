using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.DTOs.UserQuizAttemptDTOs;
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
    public interface IQuizService
    {
        Task<List<QuizResponseDto>> GetAllQuizAsync();
        Task<QuizResponseDto> GetQuizByIdAsync(Guid id);
        Task<QuizResponseDto> CreateNewQuizAsync(QuizRequestDto request);
        Task<QuizResponseDto> UpdateQuizByIdAsync(Guid id, QuizRequestDto request);
        Task<QuizResponseDto> DeleteQuizByIdAsync(Guid id);

        public QuizResponseDto MapToDTO(Quiz quiz);

        public Quiz MaptoEntity(QuizRequestDto newQuiz);

        public  Task<UserScheduleInformationResponse> GetUserScheduleInformationAsync(Guid userId, Guid scheduleId);

        public Task<PagedResult<QuizResponseDto>> GetPagedQuizzesAsync(int pageNumber, int pageSize, string? name);

        public  Task<UserQuizInformationResponse> GetUserQuizInformation(Guid userId);

        public Task<PagedResult<QuizResponseDto>> GetAllQuizByUserIdAsync(int pageNumber, int pageSize, Guid userId);
    }
}
