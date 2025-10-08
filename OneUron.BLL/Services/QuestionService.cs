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


        public QuestionService(IQuestionRepository questionRepository, IQuestionChoiceService questionChoiceService)
        {
            _questionChoiceService = questionChoiceService;
            _questionRepository = questionRepository;
        }
        public async Task<ApiResponse<List<QuestionResponseDto>>> GetAllAsync()
        {
            try
            {
                var questions = await _questionRepository.GetAllAsync();

                if (!questions.Any())
                {
                    return ApiResponse<List<QuestionResponseDto>>.FailResponse("Get All Question Fail", "Question are empty");
                }

                var result = questions.Select(MapToDTO).ToList();

                return ApiResponse<List<QuestionResponseDto>>.SuccessResponse(result, "Get All Question Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<QuestionResponseDto>>.FailResponse("Get All Question Fail", ex.Message);
            }
        }


        public async Task<ApiResponse<QuestionResponseDto>> GetbyIdAsync(Guid id)
        {
            try
            {
                var question = await _questionRepository.GetByIdAsync(id);

                if (question == null)
                {
                    return ApiResponse<QuestionResponseDto>.FailResponse("Get  Question by id Fail", "Question are not exist");
                }

                var result = MapToDTO(question);

                return ApiResponse<QuestionResponseDto>.SuccessResponse(result, "Get Question By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuestionResponseDto>.FailResponse("Get  Question by id Fail", ex.Message);
            }
        }


        public async Task<ApiResponse<QuestionResponseDto>> CreateNewQuestionAsync(QuestionRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<QuestionResponseDto>.FailResponse("Create new Question Fail", "Question are null");
                }

                var newQuestion = MapToEntity(request);

                await _questionRepository.AddAsync(newQuestion);

                var result = MapToDTO(newQuestion);

                return ApiResponse<QuestionResponseDto>.SuccessResponse(result, "Create new Question Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuestionResponseDto>.FailResponse("Create new Question Fail", ex.Message);
            }
        }


        public async Task<ApiResponse<QuestionResponseDto>> UpdateQuestionByIdAsync(Guid id, QuestionRequestDto newQuestion)
        {
            try
            {
                var existQuestion = await _questionRepository.GetbyIdAsync(id);

                if (existQuestion == null)
                {
                    return ApiResponse<QuestionResponseDto>.FailResponse("Update Question By Id Fail", "Question are not exist");
                }

                if (newQuestion == null)
                {
                    return ApiResponse<QuestionResponseDto>.FailResponse("Update Question By Id Fail", "Question are null");
                }

                existQuestion.Name = newQuestion.Name;
                existQuestion.Description = newQuestion.Description;
                existQuestion.Point = newQuestion.Point;
                existQuestion.QuizId = newQuestion.QuizId;

                await _questionRepository.UpdateAsync(existQuestion);

                var result = MapToDTO(existQuestion);

                return ApiResponse<QuestionResponseDto>.SuccessResponse(result, "Update Question By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuestionResponseDto>.FailResponse("Update Question By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<QuestionResponseDto>> DeleteQuestionByIdAsync(Guid id)
        {
            try
            {
                var existQuestion = await _questionRepository.GetbyIdAsync(id);

                if (existQuestion == null)
                {
                    return ApiResponse<QuestionResponseDto>.FailResponse("Delete Question By Id Fail", "Question are not exist");
                }

                var result = MapToDTO(existQuestion);

                await _questionRepository.DeleteAsync(existQuestion);

                return ApiResponse<QuestionResponseDto>.SuccessResponse(result, "Delete Question By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuestionResponseDto>.FailResponse("Delete Question By Id Fail", ex.Message);
            }
        }


        public Question MapToEntity(QuestionRequestDto request)
        {
            return new Question
            {
                Name = request.Name,
                Description = request.Description,
                Point = request.Point,
                QuizId = request.QuizId,
            };

        }

        public QuestionResponseDto MapToDTO(Question question)
        {
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
    }
}
