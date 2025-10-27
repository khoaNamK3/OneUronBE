using FluentValidation;
using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.DTOs.SubjectDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.ProcessRepo;
using OneUron.DAL.Repository.SubjectRepo;
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
        private readonly ISubjectRepository _subjectRepository;
        private readonly ISubjectService _subjectService;
        private readonly IProcessTaskService _taskService;
        private readonly IValidator<ProcessRequestDto> _processRequestValidator;

        public ProcessService(
            IProcessRepository processRepository,
            ISubjectService subjectService,
            IProcessTaskService taskService,
            IValidator<ProcessRequestDto> processRequestValidator,
            ISubjectRepository subjectRepository)
        {
            _processRepository = processRepository;
            _subjectService = subjectService;
            _taskService = taskService;
            _processRequestValidator = processRequestValidator;
            _subjectRepository = subjectRepository;
        }


        public async Task<List<ProcessResponseDto>> GetAllAsync()
        {
            var processes = await _processRepository.GetAllAsync();

            if (processes == null || !processes.Any())
                throw new ApiException.NotFoundException("Không tìm thấy Quá trính.");

            return processes.Select(MapToDTO).ToList();
        }

        public async Task<ProcessResponseDto> GetByIdAsync(Guid id)
        {
            var process = await _processRepository.GetByIdAsync(id);
            if (process == null)
                throw new ApiException.NotFoundException($" Quá trình với  ID {id} Không tìm thấy .");

            return MapToDTO(process);
        }


        public async Task<ProcessResponseDto> CreateProcessAsync(ProcessRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Quá trình mới không được để trống.");

            var validationResult = await _processRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);
            var newProcess = await MapToEntity(request);
            await _processRepository.AddAsync(newProcess);

            return MapToDTO(newProcess);
        }


        public async Task<ProcessResponseDto> UpdateProcessByIdAsync(Guid id, ProcessRequestDto newProcess)
        {
            var existProcess = await _processRepository.GetByIdAsync(id);
            if (existProcess == null)
                throw new ApiException.NotFoundException($"Quá trình của ID {id} Không tìm thấy.");

            if (newProcess == null)
                throw new ApiException.BadRequestException("Quá tình mới Không được để trống.");

            var validationResult = await _processRequestValidator.ValidateAsync(newProcess);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existProcess.Date = newProcess.Date;
            existProcess.Description = newProcess.Description;
            existProcess.ScheduleId = newProcess.ScheduleId;

            existProcess.Subjects.Clear();
            if (newProcess != null && newProcess.SubjectIds.Any())
            {
                foreach (var subjectId in newProcess.SubjectIds)
                {
                    var newSubject = await _subjectRepository.GetByIdAsync(subjectId);
                    if (newSubject != null)
                    {
                        existProcess.Subjects.Add(newSubject);
                    }
                }
            }

            await _processRepository.UpdateAsync(existProcess);

            return MapToDTO(existProcess);
        }


        public async Task<ProcessResponseDto> DeleteProcessByIdAsync(Guid id)
        {
            var existProcess = await _processRepository.GetByIdAsync(id);
            if (existProcess == null)
                throw new ApiException.NotFoundException($"Quá trình của ID {id} Không tìm thấy.");

            await _processRepository.DeleteAsync(existProcess);

            return MapToDTO(existProcess);
        }

        public async Task<List<ProcessResponseDto>> GetProcessesByScheduleId(Guid scheduleId)
        {
            var processes = await _processRepository.GetProcessesByScheduleId(scheduleId);

            if (!processes.Any())
                throw new ApiException.NotFoundException("Không tìm thấy quá trình gì trong lịch học này ");

            var results = processes.Select(MapToDTO).ToList();

            return results;
        }


        public ProcessResponseDto MapToDTO(Process process)
        {
            if (process == null) return null;

            return new ProcessResponseDto
            {
                Id = process.Id,
                Date = process.Date,
                Description = process.Description,
                ScheduleId = process.ScheduleId ?? Guid.Empty,

                Subjects = process.Subjects?
                    .Select(_subjectService.MapToDTO)
                    .ToList() ?? new List<SubjectResponseDto>(),

                ProcessTasks = process.ProcessTasks?
                    .Select(_taskService.MapToDTO)
                    .ToList() ?? new List<ProcessTaskResponseDto>()
            };
        }

        protected async Task<Process> MapToEntity(ProcessRequestDto request)
        {

            var utcDate = request.Date;

            if (utcDate.Kind == DateTimeKind.Unspecified)
                utcDate = DateTime.SpecifyKind(utcDate, DateTimeKind.Utc);
            else
                utcDate = utcDate.ToUniversalTime();

            var newProcess = new Process
            {
                Date = utcDate,
                Description = request.Description,
                ScheduleId = request.ScheduleId == Guid.Empty ? null : request.ScheduleId,
                Subjects = new List<Subject>()
            };

            if (request.SubjectIds != null && request.SubjectIds.Any())
            {
                foreach (var subjectId in request.SubjectIds)
                {
                    var subject = await _subjectRepository.GetByIdAsync(subjectId);
                    if (subject != null)
                    {
                        newProcess.Subjects.Add(subject);
                    }
                }
            }
            return newProcess;
        }

    }
}
