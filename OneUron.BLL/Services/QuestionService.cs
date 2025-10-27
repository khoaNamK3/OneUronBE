using FluentValidation;
using OneUron.BLL.DTOs.QuestionChoiceDTOs;
using OneUron.BLL.DTOs.QuestionDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.QuestionRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionChoiceService _questionChoiceService;
        private readonly IValidator<QuestionRequestDto> _questionRequestValidator;

        public QuestionService(
            IQuestionRepository questionRepository,
            IQuestionChoiceService questionChoiceService,
            IValidator<QuestionRequestDto> questionRequestValidator)
        {
            _questionRepository = questionRepository;
            _questionChoiceService = questionChoiceService;
            _questionRequestValidator = questionRequestValidator;
        }

       
        public async Task<List<QuestionResponseDto>> GetAllAsync()
        {
            var questions = await _questionRepository.GetAllAsync();

            if (questions == null || !questions.Any())
                throw new ApiException.NotFoundException("Không tìm thấy câu hỏi.");

            return questions.Select(MapToDTO).ToList();
        }

       
        public async Task<QuestionResponseDto> GetByIdAsync(Guid id)
        {
            var question = await _questionRepository.GetByIdAsync(id);

            if (question == null)
                throw new ApiException.NotFoundException($"Câu hỏi của  ID {id} Không tìm thấy.");

            return MapToDTO(question);
        }

       
        public async Task<QuestionResponseDto> CreateNewQuestionAsync(QuestionRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Câu hỏi mới không được để trống.");

            var validationResult = await _questionRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newQuestion = MapToEntity(request);
            await _questionRepository.AddAsync(newQuestion);

            return MapToDTO(newQuestion);
        }

       
        public async Task<QuestionResponseDto> UpdateQuestionByIdAsync(Guid id, QuestionRequestDto newQuestion)
        {
            var existQuestion = await _questionRepository.GetByIdAsync(id);
            if (existQuestion == null)
                throw new ApiException.NotFoundException($"Câu hỏi của  ID {id} Không tìm thấy.");

            if (newQuestion == null)
                throw new ApiException.BadRequestException("Câu hỏi mới không được để trống.");

            var validationResult = await _questionRequestValidator.ValidateAsync(newQuestion);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existQuestion.Name = newQuestion.Name;
            existQuestion.Description = newQuestion.Description;
            existQuestion.Point = newQuestion.Point;
            existQuestion.QuizId = newQuestion.QuizId;

            await _questionRepository.UpdateAsync(existQuestion);

            return MapToDTO(existQuestion);
        }

        
        public async Task<QuestionResponseDto> DeleteQuestionByIdAsync(Guid id)
        {
            var existQuestion = await _questionRepository.GetByIdAsync(id);
            if (existQuestion == null)
                throw new ApiException.NotFoundException($"Câu hỏi của  ID {id} Không tìm thấy.");

            await _questionRepository.DeleteAsync(existQuestion);

            return MapToDTO(existQuestion);
        }

        public QuestionResponseDto MapToDTO(Question question)
        {
            if (question == null) return null;

            return new QuestionResponseDto
            {
                Id = question.Id,
                Name = question.Name,
                Description = question.Description,
                Point = question.Point,
                QuizId = question.QuizId,

                QuestionChoices = question.QuestionChoices?
                    .Select(qc => _questionChoiceService.MapToDTO(qc))
                    .ToList() ?? new List<QuestionChoiceReponseDto>()
            };
        }

        protected Question MapToEntity(QuestionRequestDto request)
        {
            return new Question
            {
                Name = request.Name,
                Description = request.Description,
                Point = request.Point,
                QuizId = request.QuizId
            };
        }
    }
}
