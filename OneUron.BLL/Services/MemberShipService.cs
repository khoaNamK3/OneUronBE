using FluentValidation;
using OneUron.BLL.DTOs.MemberShipDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.MemberShipRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class MemberShipService : IMemberShipService
    {

        private readonly IMemberShipRepository _memberShipRepository;
        private readonly IValidator<MemberShipRequestDto> _memberShipValidator;

        public MemberShipService(IMemberShipRepository memberShipRepository, IValidator<MemberShipRequestDto> memberShipValidator)
        {
            _memberShipRepository = memberShipRepository;
            _memberShipValidator = memberShipValidator;
        }

        public async Task<List<MemberShipResponseDto>> GetAllAsync()
        {
          var memberShips = await _memberShipRepository.GetAllAsync();

            if (!memberShips.Any())
                throw new ApiException.NotFoundException("Không tìm thấy thành viên");

            var result = memberShips.Select(MapToDTO).ToList();

            return result;
        }

        public async Task<MemberShipResponseDto> GetByIdAsync(Guid id)
        {
           var existMemberShip = await _memberShipRepository.GetByIdAsync(id);

            if (existMemberShip == null)
                throw new ApiException.NotFoundException("Không tìm thấy thành viên");

            var result = MapToDTO(existMemberShip);

            return result;
        }

        public async Task<MemberShipResponseDto> CreateMemberShipAsync(MemberShipRequestDto requestDto)
        {
            var validatorResult = await _memberShipValidator.ValidateAsync(requestDto);

            if (!validatorResult.IsValid)
                throw new ApiException.ValidationException(validatorResult.Errors);

            var newMemberShip = MapToEntity(requestDto);

            await _memberShipRepository.AddAsync(newMemberShip);

            var result = MapToDTO(newMemberShip);

            return result;
        }

        public async Task<MemberShipResponseDto> UpdateMemberShipByIdAsync(Guid id, MemberShipRequestDto requestDto)
        {
            var existMemberShip = await _memberShipRepository.GetByIdAsync(id);

            if (existMemberShip == null)
                throw new ApiException.NotFoundException("Không tìm thấy thành viên");

            var validationResult = await _memberShipValidator.ValidateAsync(requestDto);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existMemberShip.ExpiredDate = requestDto.ExpiredDate;
            existMemberShip.MemberShipPlanId = requestDto.MemberShipPlanId;
            existMemberShip.StartDate = requestDto.StartDate;
            existMemberShip.Status = requestDto.Status;
            existMemberShip.UserId = requestDto.UserId;

            await _memberShipRepository.UpdateAsync(existMemberShip);

            var result = MapToDTO(existMemberShip);

            return result;
        }

        public async Task<MemberShipResponseDto> DeleteMemberShipByIdAsync(Guid id)
        {
            var existMemberShip = await _memberShipRepository.GetByIdAsync(id);

            if (existMemberShip == null)
                throw new ApiException.NotFoundException("Không tìm thấy thành viên");

            var result = MapToDTO(existMemberShip);

            await _memberShipRepository.DeleteAsync(existMemberShip);

            return result;
        }


        public MemberShip MapToEntity(MemberShipRequestDto requestDto)
        {
            return new MemberShip
            {
                ExpiredDate = requestDto.ExpiredDate,
                MemberShipPlanId = requestDto.MemberShipPlanId,
                StartDate = requestDto.StartDate,
                Status = requestDto.Status,
                UserId = requestDto.UserId,
            };
        }

        public MemberShipResponseDto MapToDTO(MemberShip memberShip)
        {
            return new MemberShipResponseDto
            {
                Id = memberShip.Id,
                ExpiredDate = memberShip.ExpiredDate,
                MemberShipPlanId = memberShip.MemberShipPlanId,
                StartDate = memberShip.StartDate,
                Status = memberShip.Status,
                UserId = memberShip.UserId,
            };
        }
    }
}
