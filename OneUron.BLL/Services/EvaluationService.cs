using FluentValidation;
using OneUron.BLL.DTOs.EnRollDTOs;
using OneUron.BLL.DTOs.EvaluationDTOs;
using OneUron.BLL.DTOs.EvaluationQuestionDTOs;
using OneUron.BLL.DTOs.InstructorDTOs;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.FluentValidation;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository;
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
        private readonly IValidator<EvaluationRequestDto> _evaluationRequestValidator;

        public EvaluationService(
            IEvaluationRepository evaluationRepository,
            IEvaluationQuestionService evaluationQuestionService,
            IMethodRuleConditionService methodRuleConditionService,
            IValidator<EvaluationRequestDto> evaluationRequestValidator)
        {
            _evaluationRepository = evaluationRepository;
            _evaluationQuestionService = evaluationQuestionService;
            _methodRuleConditionService = methodRuleConditionService;
            _evaluationRequestValidator = evaluationRequestValidator;
        }

      
        public async Task<List<EvaluationResponseDto>> GetAllAsync()
        {
            var evaluations = await _evaluationRepository.GetAllAsync();

            if (evaluations == null || !evaluations.Any())
                throw new ApiException.NotFoundException("Không tìm thấy đánh giá.");

            return evaluations.Select(MapToDTO).ToList();
        }

        public async Task<PagedResult<EvaluationPagingResponse>> GetAllPaging(int pageNumber, int pageSize, string? name)
        {
            var existEvalutions = await _evaluationRepository.GetPagingEvalutionAsync(pageNumber, pageSize, name);

            if(!existEvalutions.Items.Any())
                throw new ApiException.NotFoundException("Không tìm thấy đánh giá nào ");

            var result = existEvalutions.Items.Select(MapToPaging).ToList();

            return new PagedResult<EvaluationPagingResponse>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = result.Count,  
                Items = result
            };
        }
        
        public async Task<EvaluationResponseDto> GetByIdAsync(Guid id)
        {
            var evaluation = await _evaluationRepository.GetbyIdAsync(id);

            if (evaluation == null)
                throw new ApiException.NotFoundException($"Đánh giá của  ID {id} không tìm thấy.");

            return MapToDTO(evaluation);
        }

        
        public async Task<EvaluationResponseDto> CreateNewEvaluationAsync(EvaluationRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Đánh giá mới không được để trống.");

            var validationResult = await _evaluationRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newEvaluation = MapToEntity(request);
            await _evaluationRepository.AddAsync(newEvaluation);

            return MapToDTO(newEvaluation);
        }

      
        public async Task<EvaluationResponseDto> UpdateEvaluationByIdAsync(Guid id, EvaluationRequestDto newEvaluation)
        {
            var existEvaluation = await _evaluationRepository.GetbyIdAsync(id);
            if (existEvaluation == null)
                throw new ApiException.NotFoundException($"Đánh giá của ID {id} không tìm thấy.");

            if (newEvaluation == null)
                throw new ApiException.BadRequestException("Đánh giá mới không được để trống.");

            var validationResult = await _evaluationRequestValidator.ValidateAsync(newEvaluation);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existEvaluation.Name = newEvaluation.Name;
            existEvaluation.Description = newEvaluation.Description;
            existEvaluation.IsDeleted = newEvaluation.IsDeleted;

            await _evaluationRepository.UpdateAsync(existEvaluation);

            return MapToDTO(existEvaluation);
        }

      
        public async Task<EvaluationResponseDto> DeleteEvaluationByIdAsync(Guid id)
        {
            var existEvaluation = await _evaluationRepository.GetbyIdAsync(id);
            if (existEvaluation == null)
                throw new ApiException.NotFoundException($"Đánh giá của ID {id} không tìm thấy.");

            existEvaluation.IsDeleted = true;
            await _evaluationRepository.UpdateAsync(existEvaluation);

            return MapToDTO(existEvaluation);
        }

        public EvaluationPagingResponse MapToPaging(Evaluation evaluation)
        {
            var totalQuestion = evaluation.EvaluationQuestions.Count();
            
            return new EvaluationPagingResponse
            {
                Id = evaluation.Id,
                Name = evaluation.Name,
                Description = evaluation.Description,
                IsDeleted = evaluation.IsDeleted,
                TotalQuestion = totalQuestion
            };
        }
        
        protected Evaluation MapToEntity(EvaluationRequestDto request)
        {
            return new Evaluation
            {
                Name = request.Name,
                Description = request.Description,
                IsDeleted = request.IsDeleted
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

                // MethodRuleConditions = evaluation.MethodRuleConditions?
                //     .Select(mrc => _methodRuleConditionService.MapToDTO(mrc))
                //     .ToList() ?? new List<MethodRuleConditionResponseDto>()
            };
        }
    }
}
