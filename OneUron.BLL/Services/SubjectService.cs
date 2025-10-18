using FluentValidation;
using OneUron.BLL.DTOs.ScheduleDTOs;
using OneUron.BLL.DTOs.StudyMethodDTOs;
using OneUron.BLL.DTOs.SubjectDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.SubjectRepo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OneUron.BLL.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;
        private readonly IValidator<SubjectRequestDto> _subjectRequestValidator;
        private readonly IValidator<SubjectListRequest> _subjectListRequestValidator;
        public SubjectService(ISubjectRepository subjectRepository, IValidator<SubjectRequestDto> subjectRequestValidator, IValidator<SubjectListRequest> subjectListRequestValidator)
        {
            _subjectRepository = subjectRepository;
            _subjectRequestValidator = subjectRequestValidator;
            _subjectListRequestValidator = subjectListRequestValidator;
        }


        public async Task<List<SubjectResponseDto>> GetAllAsync()
        {
            var subjects = await _subjectRepository.GetAllAsync();

            if (subjects == null || !subjects.Any())
                throw new ApiException.NotFoundException("No subjects found.");

            return subjects.Select(MapToDTO).ToList();
        }


        public async Task<SubjectResponseDto> GetByIdAsync(Guid id)
        {
            var subject = await _subjectRepository.GetByIdAsync(id);
            if (subject == null)
                throw new ApiException.NotFoundException($"Subject with ID {id} not found.");

            return MapToDTO(subject);
        }

        public async Task<SubjectResponseDto> CreateAsync(SubjectRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Subject request cannot be null.");

            var validationResult = await _subjectRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newSubject = MapToEntity(request);
            await _subjectRepository.AddAsync(newSubject);

            return MapToDTO(newSubject);
        }

        public async Task<SubjectResponseDto> UpdateByIdAsync(Guid id, SubjectRequestDto request)
        {
            var existingSubject = await _subjectRepository.GetByIdAsync(id);
            if (existingSubject == null)
                throw new ApiException.NotFoundException($"Subject with ID {id} not found.");

            if (request == null)
                throw new ApiException.BadRequestException("Request data cannot be null.");

            var validationResult = await _subjectRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existingSubject.Name = request.Name;
            existingSubject.Priority = request.Priority;
            existingSubject.ProcessId = request.ProcessId;
            existingSubject.ScheduleId = request.ScheduleId;

            await _subjectRepository.UpdateAsync(existingSubject);

            return MapToDTO(existingSubject);
        }


        public async Task<SubjectResponseDto> DeleteByIdAsync(Guid id)
        {
            var existingSubject = await _subjectRepository.GetByIdAsync(id);
            if (existingSubject == null)
                throw new ApiException.NotFoundException($"Subject with ID {id} not found.");

            await _subjectRepository.DeleteAsync(existingSubject);
            return MapToDTO(existingSubject);
        }

        public async Task<List<SubjectResponseDto>> GetAllSubjectbyScheduleIdAsync(Guid scheduleId)
        {
            var existSubjects = await _subjectRepository.GetAllSubjectbyScheduleIdAsync(scheduleId);
            if (!existSubjects.Any())
                throw new ApiException.NotFoundException("Schedule have empty subject");

            var result = existSubjects.Select(MapToDTO).ToList();

            return result;
        }

        public Subject MapToEntity(SubjectRequestDto request)
        {
            return new Subject
            {
                Name = request.Name,
                Priority = request.Priority,
                ProcessId = request.ProcessId == Guid.Empty ? null : request.ProcessId,
                ScheduleId = request.ScheduleId
            };
        }

        public SubjectResponseDto MapToDTO(Subject subject)
        {
            if (subject == null) return null;

            return new SubjectResponseDto
            {
                Id = subject.Id,
                Name = subject.Name,
                Priority = subject.Priority,
                ProcessId = subject.ProcessId ?? Guid.Empty,
                ScheduleId = subject.ScheduleId
            };
        }

    }
}
