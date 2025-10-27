using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OneUron.BLL.DTOs.FeatureDTOs;
using OneUron.BLL.DTOs.MemberShipDTOs;
using OneUron.BLL.DTOs.MemberShipPlanDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.featureRepo;
using OneUron.DAL.Repository.MemberShipPlanRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class MemberShipPlanService : IMemberShipPlanService
    {
        private readonly IMemberShipPlanRepository _memberShipPlanRepository;
        private readonly IFeatureRepository _featureRepository;
        private readonly IValidator<MemberShipPlanRequestDto> _memberShipPlanValidator;
        private readonly IfeatureService _featureService;
        private readonly IMemberShipService _memberShipService;
        public MemberShipPlanService(IMemberShipPlanRepository memberShipPlanRepository, IValidator<MemberShipPlanRequestDto> memberShipPlanValidator,
            IfeatureService featureService, IMemberShipService memberShipService, IFeatureRepository featureRepository)
        {
            _memberShipPlanRepository = memberShipPlanRepository;
            _memberShipPlanValidator = memberShipPlanValidator;
            _featureService = featureService;
            _memberShipService = memberShipService;
            _featureRepository = featureRepository;
        }


        public async Task<List<MemberShipPlanResponseDto>> GetAllMembertShipPlanAsync()
        {
            var memberShipPlan = await _memberShipPlanRepository.GetAllMembertShipPlanAsync();

            if (!memberShipPlan.Any())
                throw new ApiException.NotFoundException("Không tìm thấy kế hoạch thành viên");

            var result = memberShipPlan.Select(MapToDTO).ToList();

            return result;
        }

        public async Task<MemberShipPlanResponseDto> GetMemberShipPlanByIdAsync(Guid id)
        {
            var existShipPlan = await _memberShipPlanRepository.GetMemberShipPlanByIdAsync(id);

            if (existShipPlan == null)
                throw new ApiException.NotFoundException("Không tìm thấy kế hoạch thành viên");

            var result = MapToDTO(existShipPlan);

            return result;
        }

        public async Task<MemberShipPlanResponseDto> CreateMemberShipPlanAsync(MemberShipPlanRequestDto requestDto)
        {
            var validationResult = await _memberShipPlanValidator.ValidateAsync(requestDto);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newMemberShip = await MapToEntityAsync(requestDto);

            await _memberShipPlanRepository.AddAsync(newMemberShip);

            var result = MapToDTO(newMemberShip);
            return result;
        }

        public async Task<MemberShipPlanResponseDto> UpdateMemberShipPlanByIdAsync(Guid id, MemberShipPlanRequestDto request)
        {
            var existShipPlan = await _memberShipPlanRepository.GetMemberShipPlanByIdAsync(id);

            if (existShipPlan == null)
                throw new ApiException.NotFoundException("Không tìm thấy kế hoạch thành viên");

            var validationResult = await _memberShipPlanValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existShipPlan.Name = request.Name;
            existShipPlan.Fee = request.Fee;
            existShipPlan.Duration = request.Duration;

            if (request.FeatureIds != null)
            {
              
                existShipPlan.Features.Clear();

 
                foreach (var featureId in request.FeatureIds)
                {
                    var existFeature = await _featureRepository.GetByIdAsync(featureId);
                    if (existFeature != null)
                    {
                        existShipPlan.Features.Add(existFeature);
                    }
                }
            }

            await _memberShipPlanRepository.UpdateAsync(existShipPlan);

            var result = MapToDTO(existShipPlan);

            return result;
        }

        public async Task<MemberShipPlanResponseDto> DeleteMemberShipPlanByIdAsync(Guid id)
        {
            var existShipPlan = await _memberShipPlanRepository.GetMemberShipPlanByIdAsync(id);

            if (existShipPlan == null)
                throw new ApiException.NotFoundException("Không tìm thấy kế hoạch thành viên");

            var result = MapToDTO(existShipPlan);

            await _memberShipPlanRepository.DeleteAsync(existShipPlan);

            return result;
        }

        public async Task<MemberShipPlan> MapToEntityAsync(MemberShipPlanRequestDto dto)
        {
            var plan = new MemberShipPlan
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Fee = dto.Fee,
                Duration = dto.Duration,
                Features = new List<Features>()
            };

            if (dto.FeatureIds != null && dto.FeatureIds.Any())
            {


                foreach (var featureId in dto.FeatureIds)
                {
                    var existFeature = await _featureRepository.GetByIdAsync(featureId);
                    if (existFeature != null)
                    {
                        plan.Features.Add(existFeature);
                    }
                }
            }

            return plan;
        }

        public MemberShipPlanResponseDto MapToDTO(MemberShipPlan memberShipPlan)
        {
            return new MemberShipPlanResponseDto
            {
                Id = memberShipPlan.Id,
                Name = memberShipPlan.Name,
                Fee = memberShipPlan.Fee,
                Duration = memberShipPlan.Duration,

                // list membership
                MemberShips = memberShipPlan.MemberShips?
            .Select(m => _memberShipService.MapToDTO(m))
            .ToList() ?? new List<MemberShipResponseDto>(),
                // list future
                Features = memberShipPlan.Features?
                .Select(f => _featureService.MapToDTO(f))
                .ToList() ?? new List<FeatureResponseDto>()
            };
        }

    }
}
