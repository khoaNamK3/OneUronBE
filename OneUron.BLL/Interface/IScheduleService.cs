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
        public Task<ApiResponse<List<ScheduleResponeDto>>> GetAllAsync();

        public Task<ApiResponse<ScheduleResponeDto>> GetByIdAsync(Guid id);

        public  Task<ApiResponse<ScheduleResponeDto>> CreateScheduleAsync(ScheduleRequestDto request);


        public  Task<ApiResponse<ScheduleResponeDto>> UpdateScheduleByIdAsync(Guid id, ScheduleRequestDto newSchedule);


        public  Task<ApiResponse<ScheduleResponeDto>> DeleteScheduleByIdAsync(Guid id);

        public ScheduleResponeDto MapToDTO(Schedule schedule);


    }
}
