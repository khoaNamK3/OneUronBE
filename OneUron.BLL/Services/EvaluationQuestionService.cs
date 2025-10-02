
using OneUron.BLL.DTOs.ChoiceDTOs;
using OneUron.BLL.DTOs.EvaluationQuestionDTOs;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.DTOs.UserAnswerDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.EvaluationQuestionRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class EvaluationQuestionService : IEvaluationQuestionService
    {
        private readonly IEvaluationQuestionRepository _evaluationQuestionRepository;

        private readonly IChoiceService _choiceService;

        private readonly IUserAnswerService _userAnswerService;

        private readonly IMethodRuleConditionService _methodRuleConditionService;
        public EvaluationQuestionService(IEvaluationQuestionRepository evaluationQuestionRepository, IChoiceService choiceService, IUserAnswerService userAnswerService, IMethodRuleConditionService methodRuleConditionService)
        {
            _evaluationQuestionRepository = evaluationQuestionRepository;
            _choiceService = choiceService;
            _userAnswerService = userAnswerService;
            _methodRuleConditionService = methodRuleConditionService;
        }

        public async Task<ApiResponse<List<EvaluationQuestionResponseDto>>> GetAllAsync()
        {
            try
            {
                var evaluationQuestions = await _evaluationQuestionRepository.GetAllAsync();

                if (!evaluationQuestions.Any())
                {
                    return ApiResponse<List<EvaluationQuestionResponseDto>>.FailResponse("Get All EvaluationQuestion Fail", "the EvaluationQuestion are empty");
                }
                var results = evaluationQuestions.Select(MapToDTO).ToList();

                return ApiResponse<List<EvaluationQuestionResponseDto>>.SuccessResponse(results, "Get All EvaluationQuestion Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<EvaluationQuestionResponseDto>>.FailResponse("Get All EvaluationQuestion Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<EvaluationQuestionResponseDto>> GetEvaluationByIdAsync(Guid id)
        {
            try
            {
                var existEvaluationQuestion = await _evaluationQuestionRepository.GetEvaluationQuestionByIdAsync(id);

                if (existEvaluationQuestion == null)
                {
                    return ApiResponse<EvaluationQuestionResponseDto>.FailResponse("Get EvaluationQuestion By Id Fail", "EvalutaionQuestion is not exist");
                }

                var result = MapToDTO(existEvaluationQuestion);

                return ApiResponse<EvaluationQuestionResponseDto>.SuccessResponse(result, "Get All EvaluationQuestion Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<EvaluationQuestionResponseDto>.FailResponse("Get EvaluationQuestion By Id Fail", ex.Message);
            }
        }


        public async Task<ApiResponse<EvaluationQuestionResponseDto>> CreateNewEvaluationQuestionAsync(EvaluationQuestionRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<EvaluationQuestionResponseDto>.FailResponse("Create New EvaluationQuestion Fail", "New EvaluationQuestion Is Null");
                }

                var newEvaluationQuestion = MapToEntity(request);

                await _evaluationQuestionRepository.AddAsync(newEvaluationQuestion);

                var result = MapToDTO(newEvaluationQuestion);

                return ApiResponse<EvaluationQuestionResponseDto>.SuccessResponse(result, "Create New EvaluationQuestion Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<EvaluationQuestionResponseDto>.FailResponse("Create New EvaluationQuestion Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<EvaluationQuestionResponseDto>> UpdateEvaluationQuestionByIdAsync(Guid id, EvaluationQuestionRequestDto newEvaluation)
        {
            try
            {
                var existEvaluationQuestion = await _evaluationQuestionRepository.GetEvaluationQuestionByIdAsync(id);

                if (existEvaluationQuestion == null)
                {
                    return ApiResponse<EvaluationQuestionResponseDto>.FailResponse("Update EvaluationQuestion By Id Fail", "EvalutaionQuestion is not exist");
                }

                if (newEvaluation == null)
                {
                    return ApiResponse<EvaluationQuestionResponseDto>.FailResponse("Update EvaluationQuestion By Id Fail", "New EvalutaionQuestion is Null");
                }

                existEvaluationQuestion.Title = newEvaluation.Title;
                existEvaluationQuestion.Description = newEvaluation.Description;
                existEvaluationQuestion.Type = newEvaluation.Type;
                existEvaluationQuestion.EvaluationId = newEvaluation.EvaluationId;

                await _evaluationQuestionRepository.UpdateAsync(existEvaluationQuestion);

                var result = MapToDTO(existEvaluationQuestion);

                return ApiResponse<EvaluationQuestionResponseDto>.SuccessResponse(result, "Update EvaluationQuestion By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<EvaluationQuestionResponseDto>.FailResponse("Update EvaluationQuestion By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<EvaluationQuestionResponseDto>> DeleteEvaluationQuestionByIdAsync(Guid id)
        {
            try
            {
                var existEvaluationQuestion = await _evaluationQuestionRepository.GetEvaluationQuestionByIdAsync(id);

                if (existEvaluationQuestion == null)
                {
                    return ApiResponse<EvaluationQuestionResponseDto>.FailResponse("Delete EvaluationQuestion By Id Fail", "EvalutaionQuestion is not exist");
                }

                var result = MapToDTO(existEvaluationQuestion);

                await _evaluationQuestionRepository.DeleteAsync(existEvaluationQuestion);

                return ApiResponse<EvaluationQuestionResponseDto>.SuccessResponse(result, "Delete EvaluationQuestion By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<EvaluationQuestionResponseDto>.FailResponse("Delete EvaluationQuestion By Id Fail", ex.Message);
            }
        }

        protected EvaluationQuestion MapToEntity(EvaluationQuestionRequestDto request)
        {
            return new EvaluationQuestion
            {
                Title = request.Title,
                Description = request.Description,
                Type = request.Type,
                EvaluationId = request.EvaluationId,
            };
        }

        public EvaluationQuestionResponseDto MapToDTO(EvaluationQuestion evaluationQuestion)
        {
            if (evaluationQuestion == null) return null;

            return new EvaluationQuestionResponseDto
            {
                Id = evaluationQuestion.Id,
                Title = evaluationQuestion.Title,
                Description = evaluationQuestion.Description,
                Type = evaluationQuestion.Type,
                EvaluationId = evaluationQuestion.EvaluationId,

                Choices = evaluationQuestion.Choices?
                    .Select(c => _choiceService.MapToDTO(c))
                    .ToList() ?? new List<ChoiceResponseDto>(),

                //userAnswers = evaluationQuestion.UserAnswers?
                //    .Select(u => _userAnswerService.MaptoDTO(u))
                //    .ToList() ?? new List<UserAnswerResponseDto>(),

                //MethodRuleConditions = evaluationQuestion.MethodRuleConditions?
                //    .Select(mrc => _methodRuleConditionService.MapToDTO(mrc))
                //    .ToList() ?? new List<MethodRuleConditionResponseDto>()
            };
        }



    }
}
