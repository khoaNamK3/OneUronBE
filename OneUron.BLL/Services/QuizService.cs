using OneUron.BLL.DTOs.QuestionDTOs;
using OneUron.BLL.DTOs.QuizDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.QuizRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepository;

        private readonly IQuestionService _questionService;

        public QuizService(IQuizRepository quizRepository, IQuestionService questionService)
        {
            _quizRepository = quizRepository;
            _questionService = questionService;
        }

        public async Task<ApiResponse<List<QuizResponseDto>>> GetAllQuizAsync()
        {
            try
            {
                var quizs = await _quizRepository.GetAllQuizAsync();

                if (!quizs.Any())
                {
                    return ApiResponse<List<QuizResponseDto>>.FailResponse("Get All Quiz Fail", "Quiz is empty");
                }
                var result = quizs.Select(MapToDTO).ToList();

                return ApiResponse<List<QuizResponseDto>>.SuccessResponse(result, "Get All Quiz Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<QuizResponseDto>>.FailResponse("Get All Quiz Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<QuizResponseDto>> GetQuizByIdAsync(Guid id)
        {
            try
            {
                var exitsQuiz = await _quizRepository.GetQuizByIdAsync(id);

                if (exitsQuiz == null)
                {
                    return ApiResponse<QuizResponseDto>.FailResponse("Get Quiz by Id Fail", "Quiz are not exist");
                }

                var result = MapToDTO(exitsQuiz);

                return ApiResponse<QuizResponseDto>.SuccessResponse(result, "Get Quiz By Id successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<QuizResponseDto>.FailResponse("Get Quiz by Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<QuizResponseDto>> CreateNewQuizAsync(QuizRequestDto reuqest)
        {
            try
            {
                if (reuqest == null)
                {
                    return ApiResponse<QuizResponseDto>.FailResponse("Create new Quiz Fail", "new Quiz is null");
                }

                var newQuiz = MaptoEntity(reuqest);

                await _quizRepository.AddAsync(newQuiz);

                var result = MapToDTO(newQuiz);

                return ApiResponse<QuizResponseDto>.SuccessResponse(result, "Create New Quizz Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<QuizResponseDto>.FailResponse("Create new Quiz Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<QuizResponseDto>> UpdateQuizByIdAsync(Guid id, QuizRequestDto newQuiz)
        {
            try
            {
                var existQuiz = await _quizRepository.GetQuizByIdAsync(id);

                if (existQuiz == null)
                {
                    return ApiResponse<QuizResponseDto>.FailResponse("Update Quiz By Id Fail", "Quiz are not exist");
                }

                if (newQuiz == null)
                {
                    return ApiResponse<QuizResponseDto>.FailResponse("Update Quiz By Id Fail", "new Quiz is empty");
                }

                existQuiz.Name = newQuiz.Name;
                existQuiz.Description = newQuiz.Description;
                existQuiz.TotalQuestion = newQuiz.TotalQuestion;
                existQuiz.TotalPoints = newQuiz.TotalPoints;
                existQuiz.Time = newQuiz.Time;
                existQuiz.Type = newQuiz.Type;
                existQuiz.PassScore = newQuiz.PassScore;
                existQuiz.UserId = newQuiz.UserId;

                await _quizRepository.UpdateAsync(existQuiz);

                var result = MapToDTO(existQuiz);

                return ApiResponse<QuizResponseDto>.SuccessResponse(result, "Update Quiz By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuizResponseDto>.FailResponse("Update Quiz By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<QuizResponseDto>> DeleteQuizByIdAsync(Guid id)
        {
            try
            {
                var existQuiz = await _quizRepository.GetByIdAsync(id);

                if (existQuiz == null)
                {
                    return ApiResponse<QuizResponseDto>.FailResponse("Delete Quiz By Id Fail", "Quiz are not exist");
                }

                var result = MapToDTO(existQuiz);

               await _quizRepository.DeleteAsync(existQuiz);

                return ApiResponse<QuizResponseDto>.SuccessResponse(result, "Delete Quiz By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<QuizResponseDto>.FailResponse("Delete Quiz By Id Fail", ex.Message);
            }
        }


        public QuizResponseDto MapToDTO(Quiz quiz)
        {
            return new QuizResponseDto
            {
                Id = quiz.Id,
                Name = quiz.Name,
                Description = quiz.Description,
                TotalQuestion = quiz.TotalQuestion,
                TotalPoints = quiz.TotalPoints,
                Time = quiz.Time,
                Type = quiz.Type,
                PassScore = quiz.PassScore,
                Questions = quiz.Questions != null
            ? quiz.Questions.Select(q => _questionService.MapToDTO(q)).ToList()
            : new List<QuestionResponseDto>(),
            UserId = quiz.UserId,
            };
        }


        public Quiz MaptoEntity(QuizRequestDto newQuiz)
        {
            return new Quiz
            {
                Description = newQuiz.Description,
                Name = newQuiz.Name,
                TotalQuestion = newQuiz.TotalQuestion,
                TotalPoints = newQuiz.TotalPoints,
                Time = newQuiz.Time,
                Type = newQuiz.Type,
                PassScore = newQuiz.PassScore,
                UserId = newQuiz.UserId,
            };
        }
    }
}
