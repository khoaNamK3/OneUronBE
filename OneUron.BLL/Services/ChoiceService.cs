using OneUron.BLL.DTOs.ChoiceDTOs;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.DTOs.UserAnswerDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.ChoiceRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class ChoiceService : IChoiceService
    {
        private readonly IChoiceRepository _choiceRepository;

        private readonly IUserAnswerService _userAnswerService;

        private readonly IMethodRuleConditionService _methodRuleConditionService;
        public ChoiceService(IChoiceRepository choiceRepository, IUserAnswerService userAnswerService, IMethodRuleConditionService methodRuleConditionService)
        {
            _choiceRepository = choiceRepository;
            _userAnswerService = userAnswerService;
            _methodRuleConditionService = methodRuleConditionService;
        }

        public async Task<ApiResponse<List<ChoiceResponseDto>>> GetAllAsync()
        {
            try
            {
                var existChoices = await _choiceRepository.GetAllAsync();

                if (!existChoices.Any())
                {
                    return ApiResponse<List<ChoiceResponseDto>>.FailResponse("Get All Choice Fail", "Choice Are Empty");
                }

                var result = existChoices.Select(MapToDTO).ToList();

                return ApiResponse<List<ChoiceResponseDto>>.SuccessResponse(result, "Get All Choice Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ChoiceResponseDto>>.FailResponse("Get All Choice Fail", ex.Message);
            }

        }

        public async Task<ApiResponse<ChoiceResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var existChoice = await _choiceRepository.GetByIdAsync(id);

                if (existChoice == null)
                {
                    return ApiResponse<ChoiceResponseDto>.FailResponse("Get Choice By Id Fail", "Choice is Not Exist");
                }
                var result = MapToDTO(existChoice);

                return ApiResponse<ChoiceResponseDto>.SuccessResponse(result, "Get Choice By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ChoiceResponseDto>.FailResponse("Get Choice By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ChoiceResponseDto>> CreateNewChoiceAsync(ChoiceRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<ChoiceResponseDto>.FailResponse("Create New Choice Fail", "New Choice is null");
                }

                var newChoice = MapToEntityDTO(request);

                await _choiceRepository.AddAsync(newChoice);

                var result = MapToDTO(newChoice);

                return ApiResponse<ChoiceResponseDto>.SuccessResponse(result, "Create New Choice Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ChoiceResponseDto>.FailResponse("Create New Choice Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ChoiceResponseDto>> UpdateChoiceByIdAsync(Guid id, ChoiceRequestDto newChoice)
        {
            try
            {
                var existChoice = await _choiceRepository.GetByIdAsync(id);

                if (existChoice == null)
                {
                    return ApiResponse<ChoiceResponseDto>.FailResponse("Update Choice By Id Fail", "Choice is Not Exist");
                }

                if (newChoice == null)
                {
                    return ApiResponse<ChoiceResponseDto>.FailResponse("Update Choice By Id Fail", "New Choice is Null");
                }

                existChoice.Description = newChoice.Description;
                existChoice.EvaluationQuestionId = newChoice.EvaluationQuestionId;
                existChoice.Title = newChoice.Title;

                await _choiceRepository.UpdateAsync(existChoice);

                var result = MapToDTO(existChoice);

                return ApiResponse<ChoiceResponseDto>.SuccessResponse(result, "Update Choice By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ChoiceResponseDto>.FailResponse("Update Choice By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<ChoiceResponseDto>> DeleteChoiceByIdAsync(Guid id)
        {
            try
            {
                var existChoice = await _choiceRepository.GetByIdAsync(id);

                if (existChoice == null)
                {
                    return ApiResponse<ChoiceResponseDto>.FailResponse("Delete Choice By Id Fail", "Choice is Not Exist");
                }

                var result = MapToDTO(existChoice);

                await _choiceRepository.DeleteAsync(existChoice);

                return ApiResponse<ChoiceResponseDto>.SuccessResponse(result, "Delete Choice Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<ChoiceResponseDto>.FailResponse("Delete Choice By Id Fail",ex.Message);
            }
        }


        protected Choice MapToEntityDTO(ChoiceRequestDto newChoice)
        {
            return new Choice
            {
                Description = newChoice.Description,
                EvaluationQuestionId = newChoice.EvaluationQuestionId,
                Title = newChoice.Title,
            };
        }


        public ChoiceResponseDto MapToDTO(Choice choice)
        {
            if (choice == null) return null;

            return new ChoiceResponseDto
            {
                Id = choice.Id,
                Description = choice.Description,
                EvaluationQuestionId = choice.EvaluationQuestionId,
                Title = choice.Title,

                //UserAnswers = choice.UserAnswers?
                //    .Select(c => _userAnswerService.MaptoDTO(c))
                //    .ToList() ?? new List<UserAnswerResponseDto>(),

                //MethodRuleConditions = choice.MethodRuleConditions?
                //    .Select(c => _methodRuleConditionService.MapToDTO(c))
                //    .ToList() ?? new List<MethodRuleConditionResponseDto>()
            };
        }

    }
}
