using FluentValidation;
using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.DTOs.ResourceDTOs;
using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.DTOs.SubjectDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.ScheduleRepo;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IProcessService _processService;
        private readonly ISubjectService _subjectService;
        private readonly IValidator<ScheduleRequestDto> _scheduleRequestValidator;
        private readonly IValidator<ScheduleSubjectRequestDto> _schedulerSubjectRequestValidator;
        private readonly IProcessTaskService _processTaskService;
        public ScheduleService(
            IScheduleRepository scheduleRepository,
            IProcessService processService,
            ISubjectService subjectService,
            IValidator<ScheduleRequestDto> scheduleRequestValidator,
            IValidator<ScheduleSubjectRequestDto> schedulerSubjectRequestValidator,
            IProcessTaskService processTaskService)
        {
            _scheduleRepository = scheduleRepository;
            _processService = processService;
            _subjectService = subjectService;
            _scheduleRequestValidator = scheduleRequestValidator;
            _schedulerSubjectRequestValidator = schedulerSubjectRequestValidator;
            _processTaskService = processTaskService;
        }


        public async Task<List<ScheduleResponeDto>> GetAllAsync()
        {
            var schedules = await _scheduleRepository.GetAllAsync();

            if (schedules == null || !schedules.Any())
                throw new ApiException.NotFoundException("No schedules found.");

            return schedules.Select(MapToDTO).ToList();
        }


        public async Task<ScheduleResponeDto> GetByIdAsync(Guid id)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(id);
            if (schedule == null)
                throw new ApiException.NotFoundException($"Schedule with ID {id} not found.");

            return MapToDTO(schedule);
        }

        public async Task<ScheduleResponeDto> CreateScheduleAsync(ScheduleRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Schedule request cannot be null.");


            var validationResult = await _scheduleRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);


            var newSchedule = MapToEntity(request);

            await _scheduleRepository.AddAsync(newSchedule);

            return MapToDTO(newSchedule);
        }


        public async Task<ScheduleResponeDto> UpdateScheduleByIdAsync(Guid id, ScheduleRequestDto newSchedule)
        {
            var existingSchedule = await _scheduleRepository.GetByIdAsync(id);
            if (existingSchedule == null)
                throw new ApiException.NotFoundException($"Schedule with ID {id} not found.");

            if (newSchedule == null)
                throw new ApiException.BadRequestException("New schedule data cannot be null.");

            var validationResult = await _scheduleRequestValidator.ValidateAsync(newSchedule);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existingSchedule.Title = newSchedule.Title;
            existingSchedule.StartDate = newSchedule.StartDate;
            existingSchedule.EndDate = newSchedule.EndDate;
            existingSchedule.TotalTime = newSchedule.TotalTime;
            existingSchedule.AmountSubject = newSchedule.AmountSubject;
            existingSchedule.CreateAt = newSchedule.CreateAt;
            existingSchedule.IsDeleted = false;
            existingSchedule.UserId = newSchedule.UserId;

            await _scheduleRepository.UpdateAsync(existingSchedule);

            return MapToDTO(existingSchedule);
        }

        public async Task<ScheduleWeekRespone> GetScheduleWeekInFormationAsync(Guid id)
        {
            var existSchedule = await _scheduleRepository.GetByIdAsync(id);

            if (existSchedule == null)
                throw new ApiException.NotFoundException($"Schedule with ID {id} not found.");

            var processes = existSchedule.Processes?.ToList() ?? new List<Process>();
            if (!processes.Any())
                throw new ApiException.NotFoundException("Schedule has no processes.");

          
            DateTime today = DateTime.UtcNow.Date;

           
            DateTime startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);
            DateTime endOfWeek = startOfWeek.AddDays(7);

            int taskOnDay = 0;
            double totalTimeOfDay = 0;
            int totalTaskOfWeek = 0;
            int totalTaskCount = 0;
            int completedTaskCount = 0;

            foreach (var process in processes)
            {
                if (process.ProcessTasks == null || !process.ProcessTasks.Any())
                    continue;

                if (process.Date.Date == today)
                {
                    foreach (var task in process.ProcessTasks)
                    {
                        taskOnDay++;
                        if (task.EndTime > task.StartTime)
                            totalTimeOfDay += (task.EndTime - task.StartTime).TotalHours;
                    }
                }


                if (process.Date.Date >= startOfWeek && process.Date.Date < endOfWeek)
                {
                    totalTaskOfWeek += process.ProcessTasks.Count;
                }


                totalTaskCount += process.ProcessTasks.Count;
                completedTaskCount += process.ProcessTasks.Count(t => t.IsCompleted);
            }

            double percentComplete = totalTaskCount == 0
                ? 0
                : (double)completedTaskCount / totalTaskCount * 100;

            return new ScheduleWeekRespone
            {
                TaskOnDay = taskOnDay,
                TotalTaskOfWeek = totalTaskOfWeek,
                TotalTimeOfDay = Math.Round(totalTimeOfDay, 2),
                PercentComplete = Math.Round(percentComplete, 2)
            };
        }

        public async Task<ScheduleResponeDto> DeleteScheduleByIdAsync(Guid id)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(id);
            if (schedule == null)
                throw new ApiException.NotFoundException($"Schedule with ID {id} not found.");

            if (schedule.IsDeleted)
                throw new ApiException.BussinessException("Schedule is already deleted.");

            schedule.IsDeleted = true;
            await _scheduleRepository.UpdateAsync(schedule);

            return MapToDTO(schedule);
        }


        public ScheduleResponeDto MapToDTO(Schedule schedule)
        {
            return new ScheduleResponeDto
            {
                Id = schedule.Id,
                Title = schedule.Title,
                StartDate = schedule.StartDate,
                EndDate = schedule.EndDate,
                TotalTime = schedule.TotalTime,
                AmountSubject = schedule.AmountSubject,
                CreateAt = schedule.CreateAt,
                IsDeleted = schedule.IsDeleted,
                UserId = schedule.UserId,

                Processes = schedule.Processes?.Select(_processService.MapToDTO).ToList() ?? new(),
                Subjects = schedule.Subjects?.Select(_subjectService.MapToDTO).ToList() ?? new()
            };
        }

        public Schedule MapToEntity(ScheduleRequestDto reuqest)
        {
            return new Schedule
            {
                Title = reuqest.Title,
                StartDate = reuqest.StartDate,
                EndDate = reuqest.EndDate,
                TotalTime = reuqest.TotalTime,
                AmountSubject = reuqest.AmountSubject,
                CreateAt = reuqest.CreateAt,
                IsDeleted = false,
                UserId = reuqest.UserId,

            };
        }
    }
}
