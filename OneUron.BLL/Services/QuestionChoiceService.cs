using OneUron.BLL.DTOs.QuestionChoiceDTOs;
using OneUron.BLL.ExceptionHandle;
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


        public QuestionChoiceService(IQuestionChoiceRepository questionChoiceRepository)
        {
            _questionChoiceRepository = questionChoiceRepository;
        }

        public async Task<ApiResponse<List<QuestionChoiceReponseDto>>> GetAllQuestionChoiceAsync()
        {
            try
            {
                var questionChoices = await _questionChoiceRepository.GetAllQuestionChoiceAsync();

                if (!questionChoices.Any())
                {
                    return ApiResponse<List<QuestionChoiceReponseDto>>.FailResponse("Get All QuestionChoice Fail", "QuestionChoice are empty");
                }

                var result = questionChoices.Select(MapToDTO).ToList();

                return ApiResponse<List<QuestionChoiceReponseDto>>.SuccessResponse(result, "Get All QuestionChoice Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<QuestionChoiceReponseDto>>.FailResponse("Get All QuestionChoice Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<QuestionChoiceReponseDto>> GetQuestionChoiceByIdAsync(Guid id)
        {
            try
            {
                var existQuestionChoice = await _questionChoiceRepository.GetQuestionChoiceByIdAsync(id);

                if (existQuestionChoice == null)
                {
                    return ApiResponse<QuestionChoiceReponseDto>.FailResponse("Get QuestionChoice By Id Fail", "QuestionChoice are not exist");
                }
                var result = MapToDTO(existQuestionChoice);

                return ApiResponse<QuestionChoiceReponseDto>.SuccessResponse(result, "Get QuestionChoice By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuestionChoiceReponseDto>.FailResponse("Get QuestionChoice By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<QuestionChoiceReponseDto>> CreateNewQuestionChoiceAsync(QuestionChoiceRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<QuestionChoiceReponseDto>.FailResponse("Create new QuestionChoice Fail", "QuestionChoice is null");
                }

                var newQuestionChoice = MapToEntity(request);

                await _questionChoiceRepository.AddAsync(newQuestionChoice);

                var result = MapToDTO(newQuestionChoice);

                return ApiResponse<QuestionChoiceReponseDto>.SuccessResponse(result, "Create New QuestionChoice Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuestionChoiceReponseDto>.FailResponse("Create new QuestionChoice Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<QuestionChoiceReponseDto>> UpdateQuestionChoiceByIdAsync(Guid id, QuestionChoiceRequestDto newQuestionChoice)
        {
            try
            {
                var existQuestionChoice = await _questionChoiceRepository.GetQuestionChoiceByIdAsync(id);
                if (existQuestionChoice == null)
                {
                    return ApiResponse<QuestionChoiceReponseDto>.FailResponse("Update QuestionChoice By Id Fail", "QuestionChoice are not exist");
                }

                if (newQuestionChoice == null)
                {
                    return ApiResponse<QuestionChoiceReponseDto>.FailResponse("Update QuestionChoice By Id Fail", "New QuestionChoice are null");
                }

                existQuestionChoice.Name = newQuestionChoice.Name;
                existQuestionChoice.IsCorrect = newQuestionChoice.IsCorrect;
                existQuestionChoice.QuestionId = newQuestionChoice.QuestionId;

                await _questionChoiceRepository.UpdateAsync(existQuestionChoice);

                var result = MapToDTO(existQuestionChoice);

                return ApiResponse<QuestionChoiceReponseDto>.SuccessResponse(result, "Update QuestionChoice By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuestionChoiceReponseDto>.FailResponse("Update QuestionChoice By Id Fail", ex.Message);
            }
        }


        public async Task<ApiResponse<QuestionChoiceReponseDto>> DeleteQuestionChoiceByIdAsync(Guid id)
        {
            try
            {
                var existQuestionChoice = await _questionChoiceRepository.GetQuestionChoiceByIdAsync(id);
                if (existQuestionChoice == null)
                {
                    return ApiResponse<QuestionChoiceReponseDto>.FailResponse("Delete QuestionChoice By Id Fail", "QuestionChoice are not exist");
                }

                var result = MapToDTO(existQuestionChoice);

                await _questionChoiceRepository.DeleteAsync(existQuestionChoice);

                return ApiResponse<QuestionChoiceReponseDto>.SuccessResponse(result, "Delete QuestionChoice By Id Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<QuestionChoiceReponseDto>.FailResponse("Delete QuestionChoice By Id Fail",ex.Message);
            }
        }

        public QuestionChoice MapToEntity(QuestionChoiceRequestDto requestDto)
        {
            return new QuestionChoice
            {
                Name = requestDto.Name,
                IsCorrect = requestDto.IsCorrect,
                QuestionId = requestDto.QuestionId,
            };
        }

        public QuestionChoiceReponseDto MapToDTO(QuestionChoice question)
        {
            return new QuestionChoiceReponseDto
            {
                Id = question.Id,
                Name = question.Name,
                IsCorrect = question.IsCorrect,
            };
        }
    }
}
