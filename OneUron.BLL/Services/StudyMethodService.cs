using FluentValidation;
using OneUron.BLL.DTOs.SkillDTOs;
using OneUron.BLL.DTOs.StudyMethodDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.StudyMethodRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class StudyMethodService : IStudyMethodService
    {
        private readonly IStudyMethodRepository _studyMethodRepository;
        private readonly IValidator<StudyMethodRequestDto> _studyMethodRequestValidator;

        public StudyMethodService(
            IStudyMethodRepository studyMethodRepository,
            IValidator<StudyMethodRequestDto> studyMethodRequestValidator)
        {
            _studyMethodRepository = studyMethodRepository;
            _studyMethodRequestValidator = studyMethodRequestValidator;
        }


        public async Task<List<StudyMethodResponseDto>> GetAllAsync()
        {
            var studyMethods = await _studyMethodRepository.GetALlAsync();

            if (studyMethods == null || !studyMethods.Any())
                throw new ApiException.NotFoundException("No study methods found.");

            return studyMethods.Select(MapToDTO).ToList();
        }


        public async Task<StudyMethodResponseDto> GetByIdAsync(Guid id)
        {
            var studyMethod = await _studyMethodRepository.GetByIdAsync(id);

            if (studyMethod == null)
                throw new ApiException.NotFoundException($"Study method with ID {id} not found.");

            return MapToDTO(studyMethod);
        }

        public async Task<StudyMethodResponseDto> GetStudyMethodByUserIdAsync(Guid userId)
        {
            var existStudyMethod = await _studyMethodRepository.GetStudyMethodByUserIdAsync(userId);

            if (existStudyMethod == null)
                throw new ApiException.NotFoundException($"Study method with ID {userId} not found.");

            return MapToDTO(existStudyMethod);
        }

        public async Task<StudyMethodResponseDto> CreateAsync(StudyMethodRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Study method request cannot be null.");

            var validationResult = await _studyMethodRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newStudyMethod = MapToEntity(request);
            newStudyMethod.UpdateDate = DateTime.UtcNow;

            await _studyMethodRepository.AddAsync(newStudyMethod);

            return MapToDTO(newStudyMethod);
        }


        public async Task<StudyMethodResponseDto> UpdateByIdAsync(Guid id, StudyMethodRequestDto request)
        {
            var existingStudyMethod = await _studyMethodRepository.GetByIdAsync(id);
            if (existingStudyMethod == null)
                throw new ApiException.NotFoundException($"Study method with ID {id} not found.");

            if (request == null)
                throw new ApiException.BadRequestException("Request data cannot be null.");

            var validationResult = await _studyMethodRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existingStudyMethod.UpdateDate = DateTime.UtcNow;
            existingStudyMethod.IsDeleted = request.IsDeleted;
            existingStudyMethod.MethodId = request.MethodId;
            existingStudyMethod.UserId = request.UserId;

            await _studyMethodRepository.UpdateAsync(existingStudyMethod);

            return MapToDTO(existingStudyMethod);
        }


        public async Task<StudyMethodResponseDto> DeleteByIdAsync(Guid id)
        {
            var existingStudyMethod = await _studyMethodRepository.GetByIdAsync(id);
            if (existingStudyMethod == null)
                throw new ApiException.NotFoundException($"Study method with ID {id} not found.");

            existingStudyMethod.IsDeleted = true;
            existingStudyMethod.UpdateDate = DateTime.UtcNow;

            await _studyMethodRepository.UpdateAsync(existingStudyMethod);

            return MapToDTO(existingStudyMethod);
        }

        
        protected StudyMethod MapToEntity(StudyMethodRequestDto request)
        {
            return new StudyMethod
            {
                IsDeleted = request.IsDeleted,
                MethodId = request.MethodId,
                UserId = request.UserId
            };
        }

        
        public StudyMethodResponseDto MapToDTO(StudyMethod studyMethod)
        {
            if (studyMethod == null) return null;

            return new StudyMethodResponseDto
            {
                Id = studyMethod.Id,
                UpdateDate = studyMethod.UpdateDate,
                IsDeleted = studyMethod.IsDeleted,
                MethodId = studyMethod.MethodId,
                UserId = studyMethod.UserId
            };
        }

       
    }
}
