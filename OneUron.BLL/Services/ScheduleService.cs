using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.DTOs.SubjectDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.ScheduleRepo;
using System;
using System.Collections.Generic;
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
        public ScheduleService(IScheduleRepository scheduleRepository, IProcessService processService, ISubjectService subjectService)
        {
            _scheduleRepository = scheduleRepository;
            _processService = processService;
            _subjectService = subjectService;
        }
        public async Task<ApiResponse<List<ScheduleResponeDto>>> GetAllAsync()
        {
            try
            {
                var schedules = await _scheduleRepository.GetAllAsync();

                if (!schedules.Any())
                {
                    return ApiResponse<List<ScheduleResponeDto>>.FailResponse("Get  All Schedule Fail", "Schedule are empty");
                }

                var result = schedules.Select(MapToDTO).ToList();

                return ApiResponse<List<ScheduleResponeDto>>.SuccessResponse(result, "Get All Schedule successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<ScheduleResponeDto>>.FailResponse("Get  All Schedule Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ScheduleResponeDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var schedule = await _scheduleRepository.GetByIdAsync(id);

                if (schedule == null)
                {
                    return ApiResponse<ScheduleResponeDto>.FailResponse("Get Schedule by Id fail", "schedule are not exist");
                }

                var result = MapToDTO(schedule);

                return ApiResponse<ScheduleResponeDto>.SuccessResponse(result, "Get Schedule By Id Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<ScheduleResponeDto>.FailResponse("Get Schedule by Id fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ScheduleResponeDto>> CreateScheduleAsync(ScheduleRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<ScheduleResponeDto>.FailResponse("Create New Schedule Fail", "Schedule are null");
                }

                var newSchedule = MapToEntity(request);

                await _scheduleRepository.AddAsync(newSchedule);

                var result = MapToDTO(newSchedule);

                return ApiResponse<ScheduleResponeDto>.SuccessResponse(result, "Create new Schedule Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ScheduleResponeDto>.FailResponse("Create New Schedule Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ScheduleResponeDto>> UpdateScheduleByIdAsync(Guid id, ScheduleRequestDto newSchedule)
        {
            try
            {
                var exsitScheDule = await _scheduleRepository.GetByIdAsync(id);

                if (exsitScheDule == null)
                {
                    return ApiResponse<ScheduleResponeDto>.FailResponse("Update Schedule By Id fail", "Schedule are no exist");
                }

                if (newSchedule == null)
                {
                    return ApiResponse<ScheduleResponeDto>.FailResponse("Update Schedule By Id fail", "New Schedule is null");
                }

                exsitScheDule.Title = newSchedule.Title;
                exsitScheDule.StartDate = newSchedule.StartDate;
                exsitScheDule.EndDate = newSchedule.EndDate;
                exsitScheDule.TotalTime = newSchedule.TotalTime;
                exsitScheDule.AmountSubject = newSchedule.AmountSubject;
                exsitScheDule.CreateAt = newSchedule.CreateAt;
                exsitScheDule.IsDeleted = false;
                exsitScheDule.UserId = newSchedule.UserId;

                await _scheduleRepository.UpdateAsync(exsitScheDule);

                var result = MapToDTO(exsitScheDule);

                return ApiResponse<ScheduleResponeDto>.SuccessResponse(result, "Update Schedule By Id successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ScheduleResponeDto>.FailResponse("Update Schedule By Id fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ScheduleResponeDto>> DeleteScheduleByIdAsync(Guid id)
        {
            try
            {
                var exsitScheDule = await _scheduleRepository.GetByIdAsync(id);

                if (exsitScheDule == null)
                {
                    return ApiResponse<ScheduleResponeDto>.FailResponse("Delete Schedule By Id fail", "Schedule are no exist");
                }

                exsitScheDule.IsDeleted = true;

                await _scheduleRepository.UpdateAsync(exsitScheDule);

                var result = MapToDTO(exsitScheDule);

                return ApiResponse<ScheduleResponeDto>.SuccessResponse(result, "Delete Schedule By id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ScheduleResponeDto>.FailResponse("Delete Schedule By Id fail",ex.Message);
            }
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
