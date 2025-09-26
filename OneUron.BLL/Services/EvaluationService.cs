using OneUron.BLL.DTOs.EvaluationDTOs;
using OneUron.BLL.DTOs.EvaluationQuestionDTOs;
using OneUron.BLL.DTOs.InstructorDTOs;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.EvaluationRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class EvaluationService : IEvaluationService
    {
        private readonly IEvaluationRepository _evaluationRepository;
        private readonly IEvaluationQuestionService _evaluationQuestionService;
        private readonly IMethodRuleConditionService _methodRuleConditionService;
        public EvaluationService(IEvaluationRepository evaluationRepository, IEvaluationQuestionService evaluationQuestionService, IMethodRuleConditionService methodRuleConditionService)
        {
            _evaluationRepository = evaluationRepository;
            _evaluationQuestionService = evaluationQuestionService;
            _methodRuleConditionService = methodRuleConditionService;
        }

        public async Task<ApiResponse<List<EvaluationResponseDto>>> GetAllAsync()
        {
            try
            {
                var evaluations = await _evaluationRepository.GetAllAsync();

                if (!evaluations.Any())
                {
                    return ApiResponse<List<EvaluationResponseDto>>.FailResponse("Get All Evaluation Fail", "the Evaluation Are Empty");
                }

                var result = evaluations.Select(MapToDTO).ToList();

                return ApiResponse<List<EvaluationResponseDto>>.SuccessResponse(result, "Get All Evaluation Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<EvaluationResponseDto>>.FailResponse("Get All Evaluation Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<EvaluationResponseDto>> GetbyIdAsync(Guid id)
        {
            try
            {
                var evaluation = await _evaluationRepository.GetbyIdAsync(id);

                if (evaluation == null)
                {
                    return ApiResponse<EvaluationResponseDto>.FailResponse("Get Evaluation By Id Fail", "Evalution is Not Exist");
                }

                var result = MapToDTO(evaluation);

                return ApiResponse<EvaluationResponseDto>.SuccessResponse(result, "Get Evaluation By id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<EvaluationResponseDto>.FailResponse("Get Evaluation By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<EvaluationResponseDto>> CreateNewEvaluationAsync(EvaluationRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<EvaluationResponseDto>.FailResponse("Create New Evaluation Fail", "Evauation is Null");
                }

                var newEvaluation = MaptoEntity(request);

                await _evaluationRepository.AddAsync(newEvaluation);

                var result = MapToDTO(newEvaluation);

                return ApiResponse<EvaluationResponseDto>.SuccessResponse(result, "Create New Evaluation Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<EvaluationResponseDto>.FailResponse("Create New Evaluation Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<EvaluationResponseDto>> UpdateEvaluationbyIdAsync(Guid id, EvaluationRequestDto newEvaluation)
        {
            try
            {
                var existEvaluation = await _evaluationRepository.GetbyIdAsync(id);

                if (existEvaluation == null)
                {
                    return ApiResponse<EvaluationResponseDto>.FailResponse("Update Evaluation By Id Fail", "Evaluation is Not Exist");
                }

                if (newEvaluation == null)
                {
                    return ApiResponse<EvaluationResponseDto>.FailResponse("Update Evaluation By Id Fail", "New Evaluation is Null");
                }

                existEvaluation.Name = newEvaluation.Name;
                existEvaluation.Description = newEvaluation.Description;
                existEvaluation.IsDeleted = newEvaluation.IsDeleted;

                await _evaluationRepository.UpdateAsync(existEvaluation);

                var result = MapToDTO(existEvaluation);

                return ApiResponse<EvaluationResponseDto>.SuccessResponse(result, "Update Evaluation Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<EvaluationResponseDto>.FailResponse("Update Evaluation By Id Fail", ex.Message);
            }
        }


        public async Task<ApiResponse<EvaluationResponseDto>> DeleteEvaluationByIdAsync(Guid id)
        {
            try
            {
                var existEvaluation = await _evaluationRepository.GetbyIdAsync(id);

                if (existEvaluation == null)
                {
                    return ApiResponse<EvaluationResponseDto>.FailResponse("Delete Evaluation By Id Fail", "Evaluation is Not Exist");
                }

               existEvaluation.IsDeleted = true;

                var result = MapToDTO(existEvaluation);

               await _evaluationRepository.UpdateAsync(existEvaluation);

                return ApiResponse<EvaluationResponseDto>.SuccessResponse(result, "Delete Evauation by id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<EvaluationResponseDto>.FailResponse("Delete Evaluation By Id Fail", ex.Message);
            }
        }

        protected Evaluation MaptoEntity(EvaluationRequestDto request)
        {
            return new Evaluation
            {
                Name = request.Name,
                Description = request.Description,
                IsDeleted = request.IsDeleted,
            };
        }

        public EvaluationResponseDto MapToDTO(Evaluation evaluation)
        {
            if (evaluation == null) return null;

            return new EvaluationResponseDto
            {
                Id = evaluation.Id,
                Name = evaluation.Name,
                Description = evaluation.Description,
                IsDeleted = evaluation.IsDeleted,

                EvaluationQuestions = evaluation.EvaluationQuestions?
                    .Select(eq => _evaluationQuestionService.MapToDTO(eq))
                    .ToList() ?? new List<EvaluationQuestionResponseDto>(),

                MethodRuleConditions = evaluation.MethodRuleConditions?
                    .Select(mrc => _methodRuleConditionService.MapToDTO(mrc))
                    .ToList() ?? new List<MethodRuleConditionResponseDto>()
            };
        }

    }
}
