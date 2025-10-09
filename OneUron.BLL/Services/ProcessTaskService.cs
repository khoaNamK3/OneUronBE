using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.ProcessTaskRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class ProcessTaskService : IProcessTaskService
    {
        private readonly IProcessTaskRepository _processTaskRepository;

        public ProcessTaskService(IProcessTaskRepository processTaskRepository)
        {
            _processTaskRepository = processTaskRepository;
        }

        public async Task<ApiResponse<List<ProcessTaskResponseDto>>> GetAllAsync()
        {
            try
            {
                var processTasks = await _processTaskRepository.GetAllAsync();

                if (!processTasks.Any())
                {
                    return ApiResponse<List<ProcessTaskResponseDto>>.FailResponse("Get All ProcessTask Fail", "ProcessTask are empty");
                }

                var result = processTasks.Select(MapToDTO).ToList();

                return ApiResponse<List<ProcessTaskResponseDto>>.SuccessResponse(result, "Get All ProcessTask Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ProcessTaskResponseDto>>.FailResponse("Get All ProcessTask Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ProcessTaskResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var processTask = await _processTaskRepository.GetByIdAsync(id);

                if (processTask == null)
                {
                    return ApiResponse<ProcessTaskResponseDto>.FailResponse("Get ProcessTask By Id Fail", "ProcessTask are not exist");
                }

                var result = MapToDTO(processTask);

                return ApiResponse<ProcessTaskResponseDto>.SuccessResponse(result, "Get ProcessTask By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProcessTaskResponseDto>.FailResponse("Get ProcessTask By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ProcessTaskResponseDto>> CreateProcessTaskAsync(ProcessTaskRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<ProcessTaskResponseDto>.FailResponse("Create new ProcessTask Fail", "ProcessTask is null");
                }

                var newProcessTask = MapToEntity(request);

                await _processTaskRepository.AddAsync(newProcessTask);

                var result = MapToDTO(newProcessTask);

                return ApiResponse<ProcessTaskResponseDto>.SuccessResponse(result, "Create new ProcessTask Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProcessTaskResponseDto>.FailResponse("Create new ProcessTask Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ProcessTaskResponseDto>> UpdateProcessTaskByIdAsync(Guid id, ProcessTaskRequestDto newProcessTask)
        {
            try
            {
                var existProcessTask = await _processTaskRepository.GetByIdAsync(id);

                if (existProcessTask == null)
                {
                    return ApiResponse<ProcessTaskResponseDto>.FailResponse("Update ProcessTask By Id Fail", "ProcessTask are not exist");
                }

                if (newProcessTask == null)
                {
                    return ApiResponse<ProcessTaskResponseDto>.FailResponse("Update ProcessTask By Id Fail", "New ProcessTask is null");
                }

                existProcessTask.Title = newProcessTask.Title;
                existProcessTask.Description = newProcessTask.Description;
                existProcessTask.Note = newProcessTask.Note;
                existProcessTask.StartTime = newProcessTask.StartTime;
                existProcessTask.EndTime = newProcessTask.EndTime;
                existProcessTask.IsCompleted = newProcessTask.IsCompleted;
                existProcessTask.ProcessId = newProcessTask.ProcessId;

                await _processTaskRepository.UpdateAsync(existProcessTask);

                var result = MapToDTO(existProcessTask);

                return ApiResponse<ProcessTaskResponseDto>.SuccessResponse(result, "Update ProcessTask by Id Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<ProcessTaskResponseDto>.FailResponse("Update ProcessTask By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ProcessTaskResponseDto>> DeleteProcessTaskByIdAsync(Guid id)
        {
            try
            {
                var existProcessTask = await _processTaskRepository.GetByIdAsync(id);

                if (existProcessTask == null)
                {
                    return ApiResponse<ProcessTaskResponseDto>.FailResponse("Delete ProcessTask By Id Fail", "ProcessTask are not exist");
                }

                var result = MapToDTO(existProcessTask);

                await _processTaskRepository.DeleteAsync(existProcessTask);

                return ApiResponse<ProcessTaskResponseDto>.SuccessResponse(result, "Delete ProcessTask By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ProcessTaskResponseDto>.FailResponse("Delete ProcessTask By Id Fail",ex.Message);
            }
        }

        public ProcessTaskResponseDto MapToDTO(ProcessTask processTask)
        {
            return new ProcessTaskResponseDto
            {
                Id = processTask.Id,
                Title = processTask.Title,
                Description = processTask.Description,
                Note = processTask.Note,
                StartTime = processTask.StartTime,
                EndTime = processTask.EndTime,
                IsCompleted = processTask.IsCompleted,
                ProcessId = processTask.ProcessId,
            };
        }

        public ProcessTask MapToEntity(ProcessTaskRequestDto request)
        {
            return new ProcessTask
            {
                Title = request.Title,
                Description = request.Description,
                Note = request.Note,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsCompleted = request.IsCompleted,
                ProcessId = request.ProcessId,
            };
        }
    }
}
