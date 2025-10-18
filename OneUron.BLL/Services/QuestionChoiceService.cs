using FluentValidation;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.DTOs.QuestionChoiceDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.QuestionChoiceRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class QuestionChoiceService : IQuestionChoiceService
    {
        private readonly IQuestionChoiceRepository _questionChoiceRepository;
        private readonly IValidator<QuestionChoiceRequestDto> _questionChoiceValidator;

        public QuestionChoiceService(
            IQuestionChoiceRepository questionChoiceRepository,
            IValidator<QuestionChoiceRequestDto> questionChoiceValidator)
        {
            _questionChoiceRepository = questionChoiceRepository;
            _questionChoiceValidator = questionChoiceValidator;
        }

     
        public async Task<List<QuestionChoiceReponseDto>> GetAllQuestionChoiceAsync()
        {
            var questionChoices = await _questionChoiceRepository.GetAllQuestionChoiceAsync();

            if (questionChoices == null || !questionChoices.Any())
                throw new ApiException.NotFoundException("No QuestionChoices found.");

            return questionChoices.Select(MapToDTO).ToList();
        }

        
        public async Task<QuestionChoiceReponseDto> GetQuestionChoiceByIdAsync(Guid id)
        {
            var existQuestionChoice = await _questionChoiceRepository.GetQuestionChoiceByIdAsync(id);
            if (existQuestionChoice == null)
                throw new ApiException.NotFoundException($"QuestionChoice with ID {id} not found.");

            return MapToDTO(existQuestionChoice);
        }

       
        public async Task<QuestionChoiceReponseDto> CreateNewQuestionChoiceAsync(QuestionChoiceRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("QuestionChoice request cannot be null.");

            var validationResult = await _questionChoiceValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newQuestionChoice = MapToEntity(request);
            await _questionChoiceRepository.AddAsync(newQuestionChoice);

            return MapToDTO(newQuestionChoice);
        }

       
        public async Task<QuestionChoiceReponseDto> UpdateQuestionChoiceByIdAsync(Guid id, QuestionChoiceRequestDto newQuestionChoice)
        {
            var existQuestionChoice = await _questionChoiceRepository.GetQuestionChoiceByIdAsync(id);
            if (existQuestionChoice == null)
                throw new ApiException.NotFoundException($"QuestionChoice with ID {id} not found.");

            if (newQuestionChoice == null)
                throw new ApiException.BadRequestException("New QuestionChoice data cannot be null.");

            var validationResult = await _questionChoiceValidator.ValidateAsync(newQuestionChoice);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existQuestionChoice.Name = newQuestionChoice.Name;
            existQuestionChoice.IsCorrect = newQuestionChoice.IsCorrect;
            existQuestionChoice.QuestionId = newQuestionChoice.QuestionId;

            await _questionChoiceRepository.UpdateAsync(existQuestionChoice);

            return MapToDTO(existQuestionChoice);
        }

      
        public async Task<QuestionChoiceReponseDto> DeleteQuestionChoiceByIdAsync(Guid id)
        {
            var existQuestionChoice = await _questionChoiceRepository.GetQuestionChoiceByIdAsync(id);
            if (existQuestionChoice == null)
                throw new ApiException.NotFoundException($"QuestionChoice with ID {id} not found.");

            await _questionChoiceRepository.DeleteAsync(existQuestionChoice);
            return MapToDTO(existQuestionChoice);
        }

        public QuestionChoiceReponseDto MapToDTO(QuestionChoice entity)
        {
            if (entity == null) return null;

            return new QuestionChoiceReponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                IsCorrect = entity.IsCorrect,
            };
        }

        protected QuestionChoice MapToEntity(QuestionChoiceRequestDto request)
        {
            return new QuestionChoice
            {
                Name = request.Name,
                IsCorrect = request.IsCorrect,
                QuestionId = request.QuestionId
            };
        }
    }
}
