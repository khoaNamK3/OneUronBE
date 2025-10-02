using OneUron.BLL.DTOs.EvaluationDTOs;
using OneUron.BLL.DTOs.UserAnswerDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.UserAnswerRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly IUserAnswerRepository _userAnswerRepository;

        public UserAnswerService(IUserAnswerRepository userAnswerRepository)
        {
            _userAnswerRepository = userAnswerRepository;
        }

        public async Task<ApiResponse<List<UserAnswerResponseDto>>> GetAllAsync()
        {
            try
            {
                var existUserAnswer = await _userAnswerRepository.GetAllAsync();

                if (!existUserAnswer.Any())
                {
                    return ApiResponse<List<UserAnswerResponseDto>>.FailResponse("Get All UserAnswer Fail ", "UserAnswers Are empty");
                }
                var result = existUserAnswer.Select(MaptoDTO).ToList();

                return ApiResponse<List<UserAnswerResponseDto>>.SuccessResponse(result, "Get All UserAnswer Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<UserAnswerResponseDto>>.FailResponse("Get All UserAnswer Fail ", ex.Message);
            }
        }

        public async Task<ApiResponse<List<UserAnswerResponseDto>>> GetByListUserAnswerAsync(Guid userId, Guid eluationQuestionId)
        {
            try
            {
                var exitsUserAnswers = await _userAnswerRepository.GetByListUserAnswerAsync(userId, eluationQuestionId);

                if (exitsUserAnswers == null)
                {
                    return ApiResponse<List<UserAnswerResponseDto>>.FailResponse("Get UserAnswer By Id Fail", "UserAnswer are not exist");
                }

                var result = exitsUserAnswers.Select(MaptoDTO).ToList();

                return ApiResponse<List<UserAnswerResponseDto>>.SuccessResponse(result, "Get All UserAnswer By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<UserAnswerResponseDto>>.FailResponse("Get UserAnswer By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<UserAnswerResponseDto>> CreateNewUserAnswerAsync(UserAnswerRequestDto resquest)
        {
            try
            {
                if (resquest == null)
                {
                    return ApiResponse<UserAnswerResponseDto>.FailResponse("Create New UserAnswer Fail", "New UserAnswer is null");
                }

                var newUserAnswer = MapToEntity(resquest);

                await _userAnswerRepository.AddAsync(newUserAnswer);

                var result = MaptoDTO(newUserAnswer);

                return ApiResponse<UserAnswerResponseDto>.SuccessResponse(result, "Create New UserAnswer Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserAnswerResponseDto>.FailResponse("Create New UserAnswer Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<UserAnswerResponseDto>> UpdateUserAnswerByUserIdAsync(Guid id, UserAnswerUpdateRequestDto request)
        {
            try
            {
                var existUserAnswer = await _userAnswerRepository.GetByIdAsync(id);

                if (existUserAnswer == null)
                {
                    return ApiResponse<UserAnswerResponseDto>.FailResponse("Update New UserAnswer Fail", " UserAnswer is Not Exist");
                }

                if (request == null)
                {
                    return ApiResponse<UserAnswerResponseDto>.FailResponse("Update New UserAnswer Fail", "New UserAnswer is Null");
                }
                existUserAnswer.UserId = existUserAnswer.UserId;
                existUserAnswer.ChoiceId = request.ChoiceId;
                existUserAnswer.EvaluationQuestionId = existUserAnswer.EvaluationQuestionId;

                var result = MaptoDTO(existUserAnswer);

                await _userAnswerRepository.UpdateAsync(existUserAnswer);

                return ApiResponse<UserAnswerResponseDto>.SuccessResponse(result, "Update UserAnswer Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserAnswerResponseDto>.FailResponse("Update New UserAnswer Fail", ex.Message);
            }
        }


        public async Task<ApiResponse<UserAnswerResponseDto>> DeleteUserAnswerByAsync(Guid id)
        {
            try
            {
                var existUserAnswer = await _userAnswerRepository.GetByIdAsync(id);

                if (existUserAnswer == null)
                {
                    return ApiResponse<UserAnswerResponseDto>.FailResponse("Delete New UserAnswer Fail", " UserAnswer is Not Exist");
                }

                var result = MaptoDTO(existUserAnswer);

                await _userAnswerRepository.DeleteAsync(existUserAnswer);

                return ApiResponse<UserAnswerResponseDto>.SuccessResponse(result, "Delete UserAnswer By User Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserAnswerResponseDto>.FailResponse("Delete New UserAnswer Fail", ex.Message);
            }
        }


        public async Task<ApiResponse<List<UserAnswerResponseDto>>> SubmitAnswersAsync(List<EvaluationSubmitRequest> evaluations)
        {
            try
            {
                var results = new List<UserAnswerResponseDto>();

                foreach (var eval in evaluations)
                {
                    if (eval.Questions == null || !eval.Questions.Any())
                        continue; // bo qua vong lap va tien qua phan tu tiep theo

                    var oldAnswers = await _userAnswerRepository.GetUserAnswerByEvaluationIdAsync(eval.UserId, eval.EvaluationId);

                    if (oldAnswers != null && oldAnswers.Any())
                    {
                        await _userAnswerRepository.DeleteRangeAsync(oldAnswers);
                    }


                    foreach (var question in eval.Questions)
                    {

                        if (eval.UserId == Guid.Empty || question.EvaluationQuestionId == Guid.Empty || question.ChoiceId == Guid.Empty)
                            continue;

                        var entity = new UserAnswer
                        {
                            Id = Guid.NewGuid(),
                            UserId = eval.UserId,
                            EvaluationQuestionId = question.EvaluationQuestionId,
                            ChoiceId = question.ChoiceId
                        };

                        await _userAnswerRepository.AddAsync(entity);
                        results.Add(MaptoDTO(entity));

                    }
                }

                return ApiResponse<List<UserAnswerResponseDto>>.SuccessResponse(results, "Answers saved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<UserAnswerResponseDto>>.FailResponse("Answers save failed", ex.Message);
            }
        }

        public async Task<ApiResponse<List<UserAnswerResponseDto>>> GetAllUserAnswerByUserIdAsync(Guid userId)
        {
            try
            {
                var userAnswers = await _userAnswerRepository.GetAllUserAnswerByUserIdAsync(userId);

                if (!userAnswers.Any())
                {
                    return ApiResponse<List<UserAnswerResponseDto>>.FailResponse("Get All UserAnswer By UserId Fail", "UserAnswer are Empty");
                }

                var results = userAnswers.Select(MaptoDTO).ToList();

                return ApiResponse<List<UserAnswerResponseDto>>.SuccessResponse(results, "Get All UserAnswer By UserId Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<UserAnswerResponseDto>>.FailResponse("Get All UserAnswer By UserId Fail", ex.Message);
            }
        }

        protected UserAnswer MapToEntity(UserAnswerRequestDto resquest)
        {
            return new UserAnswer
            {
                UserId = resquest.UserId,
                ChoiceId = resquest.ChoiceId,
                EvaluationQuestionId = resquest.EvaluationQuestionId,
            };
        }

        public UserAnswerResponseDto MaptoDTO(UserAnswer userAnswer)
        {
            return new UserAnswerResponseDto
            {
                Id = userAnswer.Id,
                UserId = userAnswer.UserId,
                EvaluationQuestionId = userAnswer.EvaluationQuestionId,
                ChoiceId = userAnswer.ChoiceId,
            };
        }

    }
}
