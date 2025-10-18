using FluentValidation;
using OneUron.BLL.DTOs.CourseDetailDTOs;
using OneUron.BLL.DTOs.EnRollDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.EnRollRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class EnRollService : IEnRollService
    {
        private readonly IEnRollRepository _enRollRepository;
        private readonly IValidator<EnRollRequestDto> _enRollRequestValidator;

        public EnRollService(IEnRollRepository enRollRepository, IValidator<EnRollRequestDto> enRollRequestValidator)
        {
            _enRollRepository = enRollRepository;
            _enRollRequestValidator = enRollRequestValidator;
        }

       
        public async Task<List<EnRollResponseDto>> GetAllEnRollAsync()
        {
            var enRolls = await _enRollRepository.GetAllEnRollAsync();

            if (enRolls == null || !enRolls.Any())
                throw new ApiException.NotFoundException("No EnRoll records found.");

            return enRolls.Select(MapToDto).ToList();
        }

       
        public async Task<EnRollResponseDto> GetEnRollByIdAsync(Guid id)
        {
            var existEnRoll = await _enRollRepository.GetEnRollByIdAsync(id);
            if (existEnRoll == null)
                throw new ApiException.NotFoundException($"EnRoll with ID {id} not found.");

            return MapToDto(existEnRoll);
        }

        
        public async Task<EnRollResponseDto> CreateNewEnRollAsync(EnRollRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("EnRoll request cannot be null.");

            var validationResult = await _enRollRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newEnRoll = MapToEntity(request);

            await _enRollRepository.AddAsync(newEnRoll);

            return MapToDto(newEnRoll);
        }

       
        public async Task<EnRollResponseDto> UpdateEnRollByIdAsync(Guid id, EnRollRequestDto enRollRequestDto)
        {
            var existEnRoll = await _enRollRepository.GetEnRollByIdAsync(id);
            if (existEnRoll == null)
                throw new ApiException.NotFoundException($"EnRoll with ID {id} not found.");

            if (enRollRequestDto == null)
                throw new ApiException.BadRequestException("EnRoll update data cannot be null.");

            var validationResult = await _enRollRequestValidator.ValidateAsync(enRollRequestDto);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existEnRoll.UserId = enRollRequestDto.UserId;
            existEnRoll.ResourceId = enRollRequestDto.ResourceId;
            existEnRoll.EnrollDate = enRollRequestDto.EnrollDate;

            await _enRollRepository.UpdateAsync(existEnRoll);

            return MapToDto(existEnRoll);
        }

        
        public async Task<EnRollResponseDto> DeleteEnRollByIdAsync(Guid id)
        {
            var existEnRoll = await _enRollRepository.GetEnRollByIdAsync(id);
            if (existEnRoll == null)
                throw new ApiException.NotFoundException($"EnRoll with ID {id} not found.");

            await _enRollRepository.DeleteAsync(existEnRoll);

            return MapToDto(existEnRoll);
        }

        
        public EnRollResponseDto MapToDto(EnRoll enRoll)
        {
            if (enRoll == null) return null;

            return new EnRollResponseDto
            {
                Id = enRoll.Id,
                UserId = enRoll.UserId,
                ResourceId = enRoll.ResourceId,
                EnrollDate = enRoll.EnrollDate
            };
        }

        protected EnRoll MapToEntity(EnRollRequestDto newEnRoll)
        {
            return new EnRoll
            {
                UserId = newEnRoll.UserId,
                ResourceId = newEnRoll.ResourceId,
                EnrollDate = newEnRoll.EnrollDate
            };
        }
    }
}
