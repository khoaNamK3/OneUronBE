using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.DTOs.ScheduleDTOs;
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
        Task<QuizResponseDto> GenerateQuestionsByQuizAsync(QuizRequestDto quiz);
        //Task<ScheduleResponeDto> CreateTasksForScheduleAsync(Guid studyMethodId, ScheduleRequestDto schedule);

        public Task<ScheduleResponeDto> CreateScheduleWithListSubjectAsync(ScheduleSubjectRequestDto scheduleSubject, Guid userId);

        public Task<ProcessResponseDto> CreatProcessTaskForProcessAsync(Guid processId, ProcessTaskGenerateRequest taskGenerateRequest);
    }
}
