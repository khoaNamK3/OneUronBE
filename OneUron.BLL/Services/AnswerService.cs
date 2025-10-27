using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OneUron.BLL.DTOs.AcknowledgeDTOs;
using OneUron.BLL.DTOs.AnswerDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.AnswerRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IValidator<AnswerRequestDto> _answerRequestValidator;

        public AnswerService(
            IAnswerRepository answerRepository,
            IValidator<AnswerRequestDto> answerRequestValidator)
        {
            _answerRepository = answerRepository;
            _answerRequestValidator = answerRequestValidator;
        }

       
        public async Task<List<AnswerResponseDto>> GetAllAnswerAsync()
        {
            var answers = await _answerRepository.GetAllAnswerAsync();

            if (answers == null || !answers.Any())
                throw new ApiException.NotFoundException("Không tìm thấy câu trả lời.");

            return answers.Select(MapToDTO).ToList();
        }

        public async Task<AnswerResponseDto> GetAnswerByIdAsync(Guid id)
        {
            var existAnswer = await _answerRepository.GetByIdAsync(id);
            if (existAnswer == null)
                throw new ApiException.NotFoundException($"Câu trả lời của ID {id} không tìm thấy.");

            return MapToDTO(existAnswer);
        }

    
        public async Task<AnswerResponseDto> CreateNewAnswerAsync(AnswerRequestDto answerRequest)
        {
            if (answerRequest == null)
                throw new ApiException.BadRequestException("Dữ liệu của câu trả lời mới không được để trống.");

            var validationResult = await _answerRequestValidator.ValidateAsync(answerRequest);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newAnswer = MapToEntity(answerRequest);

            await _answerRepository.AddAsync(newAnswer);

            return MapToDTO(newAnswer);
        }

   
        public async Task<AnswerResponseDto> UpdateAnswerByIdAsync(Guid id, AnswerRequestDto newAnswer)
        {
            var existAnswer = await _answerRepository.GetByIdAsync(id);
            if (existAnswer == null)
                throw new ApiException.NotFoundException($"Câu trả lời với ID {id} không tìm thấy.");

            if (newAnswer == null)
                throw new ApiException.BadRequestException("Dữ liệu câu trả lời mới không được để trống.");

            var validationResult = await _answerRequestValidator.ValidateAsync(newAnswer);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existAnswer.QuestionChoiceId = newAnswer.QuestionChoiceId;
            existAnswer.QuestionId = newAnswer.QuestionId;
            existAnswer.UserQuizAttemptId = newAnswer.UserQuizAttemptId;

            await _answerRepository.UpdateAsync(existAnswer);

            return MapToDTO(existAnswer);
        }

      
        public async Task<AnswerResponseDto> DeleteAnswerByIdAsync(Guid id)
        {
            var existAnswer = await _answerRepository.GetByIdAsync(id);
            if (existAnswer == null)
                throw new ApiException.NotFoundException($"Câu trả lời của ID {id} không tìm thấy.");

            await _answerRepository.DeleteAsync(existAnswer);

            return MapToDTO(existAnswer);
        }

        public async Task CreateAnswerListAsync(List<AnswerRequestDto> dtoList)
        {
            if (dtoList == null || !dtoList.Any())
                return;

            var entities = dtoList.Select(dto => new Answer
            {
                Id = Guid.NewGuid(),
                QuestionId = dto.QuestionId,
                QuestionChoiceId = dto.QuestionChoiceId,
                UserQuizAttemptId = dto.UserQuizAttemptId
            }).ToList();

            await _answerRepository.CreateListUserAnswerAsync(entities);
        }
        


        public AnswerResponseDto MapToDTO(Answer answer)
        {
            return new AnswerResponseDto
            {
                Id = answer.Id,
                QuestionChoiceId = answer.QuestionChoiceId,
                QuestionId = answer.QuestionId,
                UserQuizAttemptId = answer.UserQuizAttemptId
            };
        }

        private Answer MapToEntity(AnswerRequestDto answerRequest)
        {
            return new Answer
            {
                Id = Guid.NewGuid(),
                QuestionId = answerRequest.QuestionId,
                QuestionChoiceId = answerRequest.QuestionChoiceId,
                UserQuizAttemptId = answerRequest.UserQuizAttemptId
            };
        }
    }
}
