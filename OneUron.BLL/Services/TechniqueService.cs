using FluentValidation;
using OneUron.BLL.DTOs.SubjectDTOs;
using OneUron.BLL.DTOs.TechniqueDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.TechniqueRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class TechniqueService : ITechniqueService
    {
        private readonly ITechniqueRepository _techniqueRepository;
        private readonly IValidator<TechniqueRequestDto> _techniqueRequestValidator;

        public TechniqueService(
            ITechniqueRepository techniqueRepository,
            IValidator<TechniqueRequestDto> techniqueRequestValidator)
        {
            _techniqueRepository = techniqueRepository;
            _techniqueRequestValidator = techniqueRequestValidator;
        }

      
        public async Task<List<TechniqueResponseDto>> GetAllAsync()
        {
            var techniques = await _techniqueRepository.GetAllAsync();

            if (techniques == null || !techniques.Any())
                throw new ApiException.NotFoundException("No techniques found.");

            return techniques.Select(MapToDTO).ToList();
        }

   
        public async Task<TechniqueResponseDto> GetByIdAsync(Guid id)
        {
            var technique = await _techniqueRepository.GetByIdAsync(id);
            if (technique == null)
                throw new ApiException.NotFoundException($"Technique with ID {id} not found.");

            return MapToDTO(technique);
        }


        public async Task<TechniqueResponseDto> CreateAsync(TechniqueRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Technique request cannot be null.");

            var validationResult = await _techniqueRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newTechnique = MapToEntity(request);
            await _techniqueRepository.AddAsync(newTechnique);

            return MapToDTO(newTechnique);
        }


        public async Task<TechniqueResponseDto> UpdateByIdAsync(Guid id, TechniqueRequestDto request)
        {
            var existingTechnique = await _techniqueRepository.GetByIdAsync(id);
            if (existingTechnique == null)
                throw new ApiException.NotFoundException($"Technique with ID {id} not found.");

            if (request == null)
                throw new ApiException.BadRequestException("Request data cannot be null.");

            var validationResult = await _techniqueRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existingTechnique.Name = request.Name;
            existingTechnique.MethodId = request.MethodId;

            await _techniqueRepository.UpdateAsync(existingTechnique);

            return MapToDTO(existingTechnique);
        }

        public async Task<TechniqueResponseDto> DeleteByIdAsync(Guid id)
        {
            var existingTechnique = await _techniqueRepository.GetByIdAsync(id);
            if (existingTechnique == null)
                throw new ApiException.NotFoundException($"Technique with ID {id} not found.");

            await _techniqueRepository.DeleteAsync(existingTechnique);
            return MapToDTO(existingTechnique);
        }

        protected Technique MapToEntity(TechniqueRequestDto request)
        {
            return new Technique
            {
                Name = request.Name,
                MethodId = request.MethodId
            };
        }

        public TechniqueResponseDto MapToDTO(Technique technique)
        {
            if (technique == null) return null;

            return new TechniqueResponseDto
            {
                Id = technique.Id,
                Name = technique.Name,
                MethodId = technique.MethodId
            };
        }
    }
}
