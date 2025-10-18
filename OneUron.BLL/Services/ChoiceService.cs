using FluentValidation;
using OneUron.BLL.DTOs.AnswerDTOs;
using OneUron.BLL.DTOs.ChoiceDTOs;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.DTOs.UserAnswerDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.ChoiceRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class ChoiceService : IChoiceService
    {
        private readonly IChoiceRepository _choiceRepository;
        private readonly IUserAnswerService _userAnswerService;
        private readonly IMethodRuleConditionService _methodRuleConditionService;
        private readonly IValidator<ChoiceRequestDto> _choiceRequestValidator;

        public ChoiceService(
            IChoiceRepository choiceRepository,
            IUserAnswerService userAnswerService,
            IMethodRuleConditionService methodRuleConditionService,
            IValidator<ChoiceRequestDto> choiceRequestValidator)
        {
            _choiceRepository = choiceRepository;
            _userAnswerService = userAnswerService;
            _methodRuleConditionService = methodRuleConditionService;
            _choiceRequestValidator = choiceRequestValidator;
        }

   
        public async Task<List<ChoiceResponseDto>> GetAllAsync()
        {
            var existChoices = await _choiceRepository.GetAllAsync();

            if (existChoices == null || !existChoices.Any())
                throw new ApiException.NotFoundException("No choices found.");

            return existChoices.Select(MapToDTO).ToList();
        }


        public async Task<ChoiceResponseDto> GetByIdAsync(Guid id)
        {
            var existChoice = await _choiceRepository.GetByIdAsync(id);
            if (existChoice == null)
                throw new ApiException.NotFoundException($"Choice with ID {id} not found.");

            return MapToDTO(existChoice);
        }


        public async Task<ChoiceResponseDto> CreateNewChoiceAsync(ChoiceRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Choice request cannot be null.");

            var validationResult = await _choiceRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newChoice = MapToEntityDTO(request);

            await _choiceRepository.AddAsync(newChoice);

            return MapToDTO(newChoice);
        }


        public async Task<ChoiceResponseDto> UpdateChoiceByIdAsync(Guid id, ChoiceRequestDto newChoice)
        {
            var existChoice = await _choiceRepository.GetByIdAsync(id);
            if (existChoice == null)
                throw new ApiException.NotFoundException($"Choice with ID {id} not found.");

            if (newChoice == null)
                throw new ApiException.BadRequestException("New choice data cannot be null.");

            var validationResult = await _choiceRequestValidator.ValidateAsync(newChoice);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existChoice.Description = newChoice.Description;
            existChoice.EvaluationQuestionId = newChoice.EvaluationQuestionId;
            existChoice.Title = newChoice.Title;

            await _choiceRepository.UpdateAsync(existChoice);

            return MapToDTO(existChoice);
        }


        public async Task<ChoiceResponseDto> DeleteChoiceByIdAsync(Guid id)
        {
            var existChoice = await _choiceRepository.GetByIdAsync(id);
            if (existChoice == null)
                throw new ApiException.NotFoundException($"Choice with ID {id} not found.");

            await _choiceRepository.DeleteAsync(existChoice);

            return MapToDTO(existChoice);
        }


        protected Choice MapToEntityDTO(ChoiceRequestDto newChoice)
        {
            return new Choice
            {
                Description = newChoice.Description,
                EvaluationQuestionId = newChoice.EvaluationQuestionId,
                Title = newChoice.Title,
            };
        }

        public ChoiceResponseDto MapToDTO(Choice choice)
        {
            if (choice == null) return null;

            return new ChoiceResponseDto
            {
                Id = choice.Id,
                Description = choice.Description,
                EvaluationQuestionId = choice.EvaluationQuestionId,
                Title = choice.Title,
             
                // UserAnswers = choice.UserAnswers?
                //     .Select(c => _userAnswerService.MaptoDTO(c))
                //     .ToList() ?? new List<UserAnswerResponseDto>(),
                //
                // MethodRuleConditions = choice.MethodRuleConditions?
                //     .Select(c => _methodRuleConditionService.MapToDTO(c))
                //     .ToList() ?? new List<MethodRuleConditionResponseDto>()
            };
        }
    }
}
