using FluentValidation;
using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
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
        private readonly IValidator<ProcessTaskRequestDto> _processTaskRequestValidator;

        public ProcessTaskService(
            IProcessTaskRepository processTaskRepository,
            IValidator<ProcessTaskRequestDto> processTaskRequestValidator)
        {
            _processTaskRepository = processTaskRepository;
            _processTaskRequestValidator = processTaskRequestValidator;
        }


        public async Task<List<ProcessTaskResponseDto>> GetAllAsync()
        {
            var processTasks = await _processTaskRepository.GetAllAsync();

            if (processTasks == null || !processTasks.Any())
                throw new ApiException.NotFoundException("No process tasks found.");

            return processTasks.Select(MapToDTO).ToList();
        }

        public async Task<List<ProcessTaskResponseDto>> GetAllProcessTaskByProcessIdAsync(Guid processId)
        {
            var existProcessTask = await _processTaskRepository.GetAllProcessTaskByProcessIdAsync(processId);

            if (!existProcessTask.Any())
                throw new ApiException.NotFoundException("No ProcessTask Found");

            var result = existProcessTask.Select(MapToDTO).ToList();
            return result;
        }

        public async Task<ProcessTaskResponseDto> GetByIdAsync(Guid id)
        {
            var processTask = await _processTaskRepository.GetByIdAsync(id);

            if (processTask == null)
                throw new ApiException.NotFoundException($"ProcessTask with ID {id} not found.");

            return MapToDTO(processTask);
        }


        public async Task<ProcessTaskResponseDto> CreateProcessTaskAsync(ProcessTaskRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("ProcessTask request cannot be null.");

            var validationResult = await _processTaskRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newProcessTask = MapToEntity(request);
            await _processTaskRepository.AddAsync(newProcessTask);

            return MapToDTO(newProcessTask);
        }


        public async Task<ProcessTaskResponseDto> UpdateProcessTaskByIdAsync(Guid id, ProcessTaskRequestDto request)
        {
            var existProcessTask = await _processTaskRepository.GetByIdAsync(id);
            if (existProcessTask == null)
                throw new ApiException.NotFoundException($"ProcessTask with ID {id} not found.");

            if (request == null)
                throw new ApiException.BadRequestException("New ProcessTask data cannot be null.");

            var validationResult = await _processTaskRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existProcessTask.Title = request.Title;
            existProcessTask.Description = request.Description;
            existProcessTask.Note = request.Note;
            existProcessTask.StartTime = request.StartTime;
            existProcessTask.EndTime = request.EndTime;
            existProcessTask.IsCompleted = request.IsCompleted;
            existProcessTask.ProcessId = request.ProcessId;

            await _processTaskRepository.UpdateAsync(existProcessTask);

            return MapToDTO(existProcessTask);
        }

        public async Task<ProcessTaskResponseDto> CompleteProcessTaskAsync(Guid processTaskId)
        {
            var existProcessTask = await _processTaskRepository.GetByIdAsync(processTaskId);

            if (existProcessTask == null)
                throw new ApiException.NotFoundException($"ProcessTask with ID {processTaskId} not found.");

            if (existProcessTask.IsCompleted)
                throw new ApiException.BadRequestException("This ProcessTask has already been completed.");

            if (existProcessTask.StartTime.Date > DateTime.Now.Date)
                throw new ApiException.BadRequestException("You cannot complete a task scheduled for a future date.");

        
            existProcessTask.IsCompleted = true;

          
            if (DateTime.Now < existProcessTask.EndTime)
                existProcessTask.EndTime = DateTime.UtcNow;

            await _processTaskRepository.UpdateAsync(existProcessTask);

            return MapToDTO(existProcessTask);
        }
        public async Task<ProcessTaskResponseDto> DeleteProcessTaskByIdAsync(Guid id)
        {
            var existProcessTask = await _processTaskRepository.GetByIdAsync(id);
            if (existProcessTask == null)
                throw new ApiException.NotFoundException($"ProcessTask with ID {id} not found.");

            await _processTaskRepository.DeleteAsync(existProcessTask);

            return MapToDTO(existProcessTask);
        }


        public ProcessTaskResponseDto MapToDTO(ProcessTask processTask)
        {
            if (processTask == null) return null;

            return new ProcessTaskResponseDto
            {
                Id = processTask.Id,
                Title = processTask.Title,
                Description = processTask.Description,
                Note = processTask.Note,
                StartTime = processTask.StartTime,
                EndTime = processTask.EndTime,
                IsCompleted = processTask.IsCompleted,
                ProcessId = processTask.ProcessId
            };
        }

        protected ProcessTask MapToEntity(ProcessTaskRequestDto request)
        {
            return new ProcessTask
            {
                Title = request.Title,
                Description = request.Description,
                Note = request.Note,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                IsCompleted = request.IsCompleted,
                ProcessId = request.ProcessId
            };
        }

        
    }
}
