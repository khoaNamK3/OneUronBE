using OneUron.BLL.DTOs.AnswerDTOs;
using OneUron.BLL.ExceptionHandle;
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

        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<ApiResponse<List<AnswerResponseDto>>> GetAllAnswerAsync()
        {
            try
            {
                var answers = await _answerRepository.GetAllAnswerAsync();


                if (!answers.Any())
                {
                    return ApiResponse<List<AnswerResponseDto>>.FailResponse("Get All Answer Fail", "Answer are empty");
                }

                var result = answers.Select(MapToDTO).ToList();

                return ApiResponse<List<AnswerResponseDto>>.SuccessResponse(result, "Get All Answer Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AnswerResponseDto>>.FailResponse("Get All Answer Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<AnswerResponseDto>> GetAnswerByIdAsyc(Guid id)
        {
            try
            {
                var existAnswer = await _answerRepository.GetByIdAsync(id);

                if (existAnswer == null)
                {
                    return ApiResponse<AnswerResponseDto>.FailResponse("Get Answer By Id Fail", "Answer is not exist");
                }

                var result = MapToDTO(existAnswer);

                return ApiResponse<AnswerResponseDto>.SuccessResponse(result, "Get Answer By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AnswerResponseDto>.FailResponse("Get Answer By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<AnswerResponseDto>> CreateNewAnswerAsync(AnswerRequestDto answerRequest)
        {
            try
            {
                if (answerRequest == null)
                {
                    return ApiResponse<AnswerResponseDto>.FailResponse("Create new Answer Fail", "Answer are null");
                }

                var newAnswer = MaptoEntity(answerRequest);

                await _answerRepository.AddAsync(newAnswer);

                var result = MapToDTO(newAnswer);

                return ApiResponse<AnswerResponseDto>.SuccessResponse(result, "Create new Answer Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AnswerResponseDto>.FailResponse("Create new Answer Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<AnswerResponseDto>> UpdateAnswerByIdAsync(Guid id, AnswerRequestDto newAnswer)
        {
            try
            {
                var existAnswer = await _answerRepository.GetByIdAsync(id);
                if (existAnswer == null)
                {
                    return ApiResponse<AnswerResponseDto>.FailResponse("Update  Answer By Id Fail", " Answer are not exist");
                }

                if (newAnswer == null)
                {
                    return ApiResponse<AnswerResponseDto>.FailResponse("Update  Answer By Id Fail", "New Answer is null");
                }

                existAnswer.QuestionChoiceId = newAnswer.QuestionChoiceId;
                existAnswer.QuestionId = newAnswer.QuestionId;
                existAnswer.UserQuizAttemptId = newAnswer.UserQuizAttemptId;

                await _answerRepository.UpdateAsync(existAnswer);

                var result = MapToDTO(existAnswer);

                return ApiResponse<AnswerResponseDto>.SuccessResponse(result, "Update  Answer By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AnswerResponseDto>.FailResponse("Update  Answer By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<AnswerResponseDto>> DeleteAnswerByIdAsync(Guid id)
        {
            try
            {
                var existAnswer = await _answerRepository.GetByIdAsync(id);
                if (existAnswer == null)
                {
                    return ApiResponse<AnswerResponseDto>.FailResponse("Delete  Answer By Id Fail", "New Answer are not exist");
                }

                var result = MapToDTO(existAnswer);

                await _answerRepository.DeleteAsync(existAnswer);

                return ApiResponse<AnswerResponseDto>.SuccessResponse(result, "Delete Answer By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AnswerResponseDto>.FailResponse("Delete new Answer By Id Fail", ex.Message);
            }
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

        public Answer MaptoEntity(AnswerRequestDto answerRequest)
        {
            return new Answer
            {
                QuestionId = answerRequest.QuestionId,
                UserQuizAttemptId = answerRequest.UserQuizAttemptId,
                QuestionChoiceId = answerRequest.QuestionChoiceId
            };
        }
    }
}
