using FluentValidation;
using OneUron.BLL.DTOs.FeatureDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.featureRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services.UserServices
{
    public class featureService : IfeatureService
    {
        private readonly IFeatureRepository _featureRepository;
        private readonly IValidator<FeatureRequestDto> _featureValidation;
        public featureService(IFeatureRepository featureRepository, IValidator<FeatureRequestDto> featureValidation)
        {
            _featureRepository = featureRepository;
            _featureValidation = featureValidation;
        }

        public async Task<List<FeatureResponseDto>> GetAllAsync()
        {
          var features = await _featureRepository.GetAllAsync();

            if (!features.Any())
                throw  new ApiException.NotFoundException("Không tìm thấy tính năng");

            var result = features.Select(MapToDTO).ToList();
            
            return result;  
        }

        public async Task<FeatureResponseDto> GetByIdAsync(Guid id)
        {
            var feature = await _featureRepository.GetByIdAsync(id);

            if (feature == null)
                throw new ApiException.NotFoundException($"Tính năng {id} không tìm thấy  ");

            var result = MapToDTO(feature);

            return result;
        }


        public async Task<FeatureResponseDto> CreateFeatureAsync(FeatureRequestDto requestDto)
        {
            var validationResult = await _featureValidation.ValidateAsync(requestDto);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newFeature = MapToEntity(requestDto);

            await _featureRepository.AddAsync(newFeature);

            var result = MapToDTO(newFeature);

            return result;
        }


        public async Task<FeatureResponseDto> UpdateFeatureByIdAsync(Guid id, FeatureRequestDto requestDto)
        {
            var feature = await _featureRepository.GetByIdAsync(id);

            if (feature == null)
                throw new ApiException.NotFoundException($"Tính năng  {id} Không tìm thấy ");

            var validationResult = await _featureValidation.ValidateAsync(requestDto);

            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

           feature.Name = requestDto.Name;
            feature.Description = requestDto.Description;

            await _featureRepository.UpdateAsync(feature);
            var result = MapToDTO(feature);
            return result;
        }

        public async Task<FeatureResponseDto> DeleteFuatureByIdAsync(Guid id)
        {
            var feature = await _featureRepository.GetByIdAsync(id);

            if (feature == null)
                throw new ApiException.NotFoundException($"Tính Năng  {id} Không tìm thấy ");

            var result = MapToDTO(feature);

            await _featureRepository.DeleteAsync(feature);

            return result;
        }


        public Features MapToEntity(FeatureRequestDto requestDto)
        {
            return new Features
            {
                Name = requestDto.Name,
                Description = requestDto.Description,
            };
        }


        public FeatureResponseDto MapToDTO(Features feature)
        {
            return new FeatureResponseDto
            {
                Id = feature.Id,
                Description = feature.Description,
                Name = feature.Name,
            };
        }
    }
}
