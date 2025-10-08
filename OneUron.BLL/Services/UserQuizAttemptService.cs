using OneUron.BLL.DTOs.AnswerDTOs;
using OneUron.BLL.DTOs.UserQuizAttemptDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.UserQuizAttemptRepo;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class UserQuizAttemptService : IUserQuizAttemptService
    {
        private readonly IUserQuizAttemptReposiotry _userQuizAttemptReposiotry;

        private readonly IAnswerService _answerService;
        public UserQuizAttemptService(IUserQuizAttemptReposiotry userQuizAttemptReposiotry, IAnswerService answerService)
        {
            _userQuizAttemptReposiotry = userQuizAttemptReposiotry;
            _answerService = answerService;
        }

        public async Task<ApiResponse<List<UserQuizAttemptResponseDto>>> GetAllUserQuizAttemptAsync()
        {
            try
            {
                var userQuizAttempts = await _userQuizAttemptReposiotry.GetAllUserQuizAttemptAsync();

                if (!userQuizAttempts.Any())
                {
                    return ApiResponse<List<UserQuizAttemptResponseDto>>.FailResponse("Get All UserQuizAttempt Fail", "UserQuizAttempt are empty");
                }

                var result = userQuizAttempts.Select(MapToDTO).ToList();

                return ApiResponse<List<UserQuizAttemptResponseDto>>.SuccessResponse(result, "Get All UserQuizAttempt Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<UserQuizAttemptResponseDto>>.FailResponse("Get All UserQuizAttempt Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<UserQuizAttemptResponseDto>> GetUserQuizAttemptsByIdAsync(Guid id)
        {
            try
            {
                var existUserQuizAttemp = await _userQuizAttemptReposiotry.GetUserQuizAttemptsByIdAsync(id);

                if (existUserQuizAttemp == null)
                {
                    return ApiResponse<UserQuizAttemptResponseDto>.FailResponse("Get UserQuizAttempt By Id Fail", "UserQuizAttempt Are not exits");
                }

                var result = MapToDTO(existUserQuizAttemp);

                return ApiResponse<UserQuizAttemptResponseDto>.SuccessResponse(result, "Get UserQuizAttempt By Id Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<UserQuizAttemptResponseDto>.FailResponse("Get UserQuizAttempt By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<UserQuizAttemptResponseDto>> CreateNewUserQuizAttemptAsync(UserQuizAttemptRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<UserQuizAttemptResponseDto>.FailResponse("Create new UserQuizAttempt Fail", "New UserQuizAttempt are empty");
                }

                var newUserQuizAttempt = MapToEntity(request);

                await _userQuizAttemptReposiotry.AddAsync(newUserQuizAttempt);

                var result = MapToDTO(newUserQuizAttempt);

                return ApiResponse<UserQuizAttemptResponseDto>.SuccessResponse(result, "Create new UserQuizAttempt Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserQuizAttemptResponseDto>.FailResponse("Create new UserQuizAttempt Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<UserQuizAttemptResponseDto>> UpdateUserQuizAttemptByIdAsync(Guid id, UserQuizAttemptRequestDto newUserQuizAttempt)
        {
            try
            {
                var existUserQuizAttempt = await _userQuizAttemptReposiotry.GetUserQuizAttemptsByIdAsync(id);

                if (existUserQuizAttempt == null)
                {
                    return ApiResponse<UserQuizAttemptResponseDto>.FailResponse("Update UserQuizAttempt By Id Fail", "UserQuizAttempt are not exist");
                }

                if (newUserQuizAttempt == null)
                {
                    return ApiResponse<UserQuizAttemptResponseDto>.FailResponse("Update UserQuizAttempt By Id Fail", "New UserQuizAttempt are null");
                }

                existUserQuizAttempt.QuizId = newUserQuizAttempt.QuizId;
                existUserQuizAttempt.StartAt = newUserQuizAttempt.StartAt;
                existUserQuizAttempt.FinishAt = newUserQuizAttempt.FinishAt;
                existUserQuizAttempt.Point = newUserQuizAttempt.Point;
                existUserQuizAttempt.Accuracy = newUserQuizAttempt.Accuracy;

                await _userQuizAttemptReposiotry.UpdateAsync(existUserQuizAttempt);

                var result = MapToDTO(existUserQuizAttempt);

                return ApiResponse<UserQuizAttemptResponseDto>.SuccessResponse(result, "Update UserQuizAttempt By Id Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<UserQuizAttemptResponseDto>.FailResponse("Update UserQuizAttempt By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<UserQuizAttemptResponseDto>> DeleteUserQuizAttemptByIdAsync(Guid id)
        {
            try
            {
                var existUserQuizAttempt = await _userQuizAttemptReposiotry.GetUserQuizAttemptsByIdAsync(id);

                if (existUserQuizAttempt == null)
                {
                    return ApiResponse<UserQuizAttemptResponseDto>.FailResponse("Delete UserQuizAttempt By Id Fail", "UserQuizAttempt are not exist");
                }

                var result = MapToDTO(existUserQuizAttempt);

                await _userQuizAttemptReposiotry.DeleteAsync(existUserQuizAttempt);

                return ApiResponse<UserQuizAttemptResponseDto>.SuccessResponse(result, "Delete UserQuizAttempt By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<UserQuizAttemptResponseDto>.FailResponse("Delete UserQuizAttempt By Id Fail", ex.Message);
            }
        }


        public UserQuizAttemptResponseDto MapToDTO(UserQuizAttempt userQuizAttempt)
        {
            return new UserQuizAttemptResponseDto
            {
                Id = userQuizAttempt.Id,
                QuizId = userQuizAttempt.QuizId,
                StartAt = userQuizAttempt.StartAt,
                FinishAt = userQuizAttempt.FinishAt,
                Point = userQuizAttempt.Point,
                Accuracy = userQuizAttempt.Accuracy,

                Answers = userQuizAttempt.Answers != null
                    ? userQuizAttempt.Answers
                        .Select(a => _answerService.MapToDTO(a))
                        .ToList()
                    : new List<AnswerResponseDto>()
            };
        }

        public UserQuizAttempt MapToEntity(UserQuizAttemptRequestDto userQuizAttemptRequestDto)
        {
            return new UserQuizAttempt
            {
                QuizId = userQuizAttemptRequestDto.QuizId,
                StartAt = userQuizAttemptRequestDto.StartAt,
                FinishAt = userQuizAttemptRequestDto.FinishAt,
                Point = userQuizAttemptRequestDto.Point,
                Accuracy = userQuizAttemptRequestDto.Accuracy,
            };
        }
    }
}
