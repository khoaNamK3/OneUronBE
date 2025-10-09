using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.ProcessRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class ProcessService : IProcessService
    {
        private readonly IProcessRepository _processRepository;
        private readonly ISubjectService _subjectService;
        private readonly IProcessTaskService _taskService;
        public ProcessService(IProcessRepository processRepository, ISubjectService subjectService, IProcessTaskService taskService)
        {
            _processRepository = processRepository;
            _subjectService = subjectService;
            _taskService = taskService;
        }

        public async Task<ApiResponse<List<ProcessResponseDto>>> GetAllAsync()
        {
            try
            {
                var processes = await _processRepository.GetAllAsync();

                if (!processes.Any())
                {
                    return ApiResponse<List<ProcessResponseDto>>.FailResponse("Get All Process Fail", "process Are empty");
                }

                var result = processes.Select(MapToDTO).ToList();

                return ApiResponse<List<ProcessResponseDto>>.SuccessResponse(result, "Get All Process Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProcessResponseDto>>.FailResponse("Get All Process Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ProcessResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var process = await _processRepository.GetByIdAsync(id);

                if (process == null)
                {
                    return ApiResponse<ProcessResponseDto>.FailResponse("Get Process By id Fail", "process are not exist");
                }

                var result = MapToDTO(process);

                return ApiResponse<ProcessResponseDto>.SuccessResponse(result, "Get Process By Id Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<ProcessResponseDto>.FailResponse("Get Process By id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ProcessResponseDto>> CreateProcessAsync(ProcessRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<ProcessResponseDto>.FailResponse("Create new Process Fail", "Process are null");
                }

                var newProcess = MapToEntity(request);

                await _processRepository.AddAsync(newProcess);

                var result = MapToDTO(newProcess);

                return ApiResponse<ProcessResponseDto>.SuccessResponse(result, "Create new Process Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProcessResponseDto>.FailResponse("Create new Process Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ProcessResponseDto>> UpdateProcessByIdAsync(Guid id, ProcessRequestDto newProcess)
        {
            try
            {
                var existProcess = await _processRepository.GetByIdAsync(id);

                if (existProcess == null)
                {
                    return ApiResponse<ProcessResponseDto>.FailResponse("Update Process By Id Fail", "Process are not exist");
                }

                if (newProcess == null)
                {
                    return ApiResponse<ProcessResponseDto>.FailResponse("Update Process By Id Fail", "New Process are null");
                }

                existProcess.Date = newProcess.Date;
                existProcess.Description = newProcess.Description;
                existProcess.ScheduleId = newProcess.ScheduleId;

                await _processRepository.UpdateAsync(existProcess);

                var result = MapToDTO(existProcess);

                return ApiResponse<ProcessResponseDto>.SuccessResponse(result, "Update Process By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProcessResponseDto>.FailResponse("Update Process By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ProcessResponseDto>> DeleteProcessByIdAsync(Guid id)
        {
            try
            {
                var existProcess = await _processRepository.GetByIdAsync(id);

                if (existProcess == null)
                {
                    return ApiResponse<ProcessResponseDto>.FailResponse("Delete Process By Id Fail", "Process are not exist");
                }

                var result = MapToDTO(existProcess);

                await _processRepository.DeleteAsync(existProcess);

                return ApiResponse<ProcessResponseDto>.SuccessResponse(result, "Delete Process By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProcessResponseDto>.FailResponse("Delete Process By Id Fail", ex.Message);
            }
        }


        public ProcessResponseDto MapToDTO(Process process)
        {
            return new ProcessResponseDto
            {
                Id = process.Id,
                Date = process.Date,
                Description = process.Description,
                ScheduleId = process.ScheduleId,


                Subjects = process.Subjects?.Select(_subjectService.MapToDTO).ToList() ?? new(),
                ProcessTasks = process.ProcessTasks?.Select(_taskService.MapToDTO).ToList() ?? new()
            };
        }

        public Process MapToEntity(ProcessRequestDto request)
        {
            return new Process
            {
                Date = request.Date,
                Description = request.Description,
                ScheduleId = request.ScheduleId,
            };
        }
    }
}
