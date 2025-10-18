using FluentValidation;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.MethodRulesRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class MethodRuleService :  IMethodRuleService
    {
        private readonly IMethodRuleRepository _methodRuleRepository;
        private readonly IValidator<MethodRuleRequestDto> _methodRuleRequestValidator;

        public MethodRuleService(
            IMethodRuleRepository methodRuleRepository,
            IValidator<MethodRuleRequestDto> methodRuleRequestValidator)
        {
            _methodRuleRepository = methodRuleRepository;
            _methodRuleRequestValidator = methodRuleRequestValidator;
        }

 
        public async Task<List<MethodRuleResponseDto>> GetAllAsync()
        {
            var methodRules = await _methodRuleRepository.GetAllAsync();

            if (methodRules == null || !methodRules.Any())
                throw new ApiException.NotFoundException("No MethodRule records found.");

            return methodRules.Select(MapToDTO).ToList();
        }

  
        public async Task<MethodRuleResponseDto> GetByIdAsync(Guid id)
        {
            var existMethodRule = await _methodRuleRepository.GetByIdAsync(id);
            if (existMethodRule == null)
                throw new ApiException.NotFoundException($"MethodRule with ID {id} not found.");

            return MapToDTO(existMethodRule);
        }

  
        public async Task<MethodRuleResponseDto> CreateNewMethodRuleAsync(MethodRuleRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("MethodRule request cannot be null.");

            var validationResult = await _methodRuleRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newMethodRule = MapToEntity(request);

            await _methodRuleRepository.AddAsync(newMethodRule);

            return MapToDTO(newMethodRule);
        }

 
        public async Task<MethodRuleResponseDto> UpdateMethodRuleByIdAsync(Guid id, MethodRuleRequestDto newMethodRule)
        {
            var existMethodRule = await _methodRuleRepository.GetByIdAsync(id);
            if (existMethodRule == null)
                throw new ApiException.NotFoundException($"MethodRule with ID {id} not found.");

            if (newMethodRule == null)
                throw new ApiException.BadRequestException("New MethodRule data cannot be null.");

            var validationResult = await _methodRuleRequestValidator.ValidateAsync(newMethodRule);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existMethodRule.MethodId = newMethodRule.MethodId;
            existMethodRule.MethodRuleConditionId = newMethodRule.MethodRuleConditionId;

            await _methodRuleRepository.UpdateAsync(existMethodRule);

            return MapToDTO(existMethodRule);
        }

 
        public async Task<MethodRuleResponseDto> DeleteMethodRuleByIdAsync(Guid id)
        {
            var existMethodRule = await _methodRuleRepository.GetByIdAsync(id);
            if (existMethodRule == null)
                throw new ApiException.NotFoundException($"MethodRule with ID {id} not found.");

            await _methodRuleRepository.DeleteAsync(existMethodRule);

            return MapToDTO(existMethodRule);
        }

        protected MethodRule MapToEntity(MethodRuleRequestDto request)
        {
            return new MethodRule
            {
                MethodId = request.MethodId,
                MethodRuleConditionId = request.MethodRuleConditionId
            };
        }

        public MethodRuleResponseDto MapToDTO(MethodRule methodRule)
        {
            if (methodRule == null) return null;

            return new MethodRuleResponseDto
            {
                Id = methodRule.Id,
                MethodId = methodRule.MethodId,
                MethodRuleConditionId = methodRule.MethodRuleConditionId
            };
        }
    }
}
