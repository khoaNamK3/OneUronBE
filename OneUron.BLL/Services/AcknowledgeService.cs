using FluentValidation;
using OneUron.BLL.DTOs.AcknowledgeDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.AcknowledgeRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class AcknowledgeService : IAcknowledgeService
    {
        private readonly IAcknowledgeRepository _acknowledgeRepository;
        private readonly IValidator<AcknowledgeRequestDto> _acknowledgeRequestValidator;

        public AcknowledgeService(
            IAcknowledgeRepository acknowledgeRepository,
            IValidator<AcknowledgeRequestDto> acknowledgeRequestValidator)
        {
            _acknowledgeRepository = acknowledgeRepository;
            _acknowledgeRequestValidator = acknowledgeRequestValidator;
        }

      
        public async Task<List<AcknowledgeResponseDto>> GetAllAcknowledgeAsync()
        {
            var acknowLedges = await _acknowledgeRepository.GetAllAcknowledgeAsync();

            if (acknowLedges == null || !acknowLedges.Any())
                throw new ApiException.NotFoundException("No Acknowledge records found.");

            return acknowLedges.Select(MapToDTO).ToList();
        }

       
        public async Task<AcknowledgeResponseDto> GetAcknowledgeByIdAsync(Guid id)
        {
            var existAcknowledge = await _acknowledgeRepository.GetAcknowledgeByIdAsync(id);
            if (existAcknowledge == null)
                throw new ApiException.NotFoundException($"Acknowledge with ID {id} not found.");

            return MapToDTO(existAcknowledge);
        }

  
        public async Task<AcknowledgeResponseDto> CreateNewAcknowledgeAsync(AcknowledgeRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Acknowledge request cannot be null.");

            var validationResult = await _acknowledgeRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newAcknowledge = MapToEnitity(request);
            await _acknowledgeRepository.AddAsync(newAcknowledge);

            return MapToDTO(newAcknowledge);
        }

      
        public async Task<AcknowledgeResponseDto> UpdateAcknowLedgeByIdAsync(Guid id, AcknowledgeRequestDto newAcknowLedge)
        {
            var existAcknowledge = await _acknowledgeRepository.GetAcknowledgeByIdAsync(id);
            if (existAcknowledge == null)
                throw new ApiException.NotFoundException($"Acknowledge with ID {id} not found.");

            if (newAcknowLedge == null)
                throw new ApiException.BadRequestException("Acknowledge data cannot be null.");

            var validationResult = await _acknowledgeRequestValidator.ValidateAsync(newAcknowLedge);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existAcknowledge.Text = newAcknowLedge.Text;
            existAcknowledge.CourseId = newAcknowLedge.CourseId;

            await _acknowledgeRepository.UpdateAsync(existAcknowledge);

            return MapToDTO(existAcknowledge);
        }

   
        public async Task<AcknowledgeResponseDto> DeleteAcknowledgeByIdAsync(Guid id)
        {
            var existAcknowledge = await _acknowledgeRepository.GetAcknowledgeByIdAsync(id);
            if (existAcknowledge == null)
                throw new ApiException.NotFoundException($"Acknowledge with ID {id} not found.");

            await _acknowledgeRepository.DeleteAsync(existAcknowledge);
            return MapToDTO(existAcknowledge);
        }


        protected Acknowledge MapToEnitity(AcknowledgeRequestDto request)
        {
            return new Acknowledge
            {
                Text = request.Text,
                CourseId = request.CourseId,
            };
        }

        public AcknowledgeResponseDto MapToDTO(Acknowledge acknowledge)
        {
            return new AcknowledgeResponseDto
            {
                Id = acknowledge.Id,
                Text = acknowledge.Text,
                CourseId = acknowledge.CourseId,
            };
        }
    }
}
