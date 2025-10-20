using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IScheduleService
    {
        Task<List<ScheduleResponeDto>> GetAllAsync();

        Task<ScheduleResponeDto> GetByIdAsync(Guid id);

        Task<ScheduleResponeDto> CreateScheduleAsync(ScheduleRequestDto request);

        Task<ScheduleResponeDto> UpdateScheduleByIdAsync(Guid id, ScheduleRequestDto newSchedule);

        Task<ScheduleResponeDto> DeleteScheduleByIdAsync(Guid id);

        public ScheduleResponeDto MapToDTO(Schedule schedule);

        public  Task<ScheduleWeekRespone> GetScheduleWeekInFormationAsync(Guid id);

        public Task<List<ScheduleResponeDto>> GetAllScheduleByUserIdAsync(Guid userId);

    }
}
