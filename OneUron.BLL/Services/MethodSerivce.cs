using OneUron.BLL.DTOs.ChoiceDTOs;
using OneUron.BLL.DTOs.MethodConDTOs;
using OneUron.BLL.DTOs.MethodDTOs;
using OneUron.BLL.DTOs.MethodProDTOs;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.BLL.DTOs.TechniqueDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.MethodRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class MethodSerivce : IMethodSerivce
    {
        private readonly IMethodRepository _methodRepository;

        private readonly IMethodProSerivce _proSerivce;

        private readonly IMethodConService _conService;

        private readonly ITechniqueService _techniqueService;

        private readonly IMethodRuleService _methodRuleService;

        private readonly IUserAnswerService _userAnswerService;

        private readonly IChoiceService _choiceService;

        private readonly IMethodRuleConditionService _methodRuleConditionService;
        public MethodSerivce(IMethodRepository methodRepository, IMethodProSerivce proSerivce, IMethodConService conService,
            ITechniqueService techniqueService, IMethodRuleService methodRuleService, IUserAnswerService userAnswerService, IChoiceService choiceService, IMethodRuleConditionService methodRuleConditionService)
        {
            _methodRepository = methodRepository;
            _proSerivce = proSerivce;
            _conService = conService;
            _techniqueService = techniqueService;
            _methodRuleService = methodRuleService;
            _userAnswerService = userAnswerService;
            _choiceService = choiceService;
            _methodRuleConditionService = methodRuleConditionService;
        }

        public async Task<ApiResponse<List<MethodResponseDto>>> GetAllAsync()
        {
            try
            {
                var methods = await _methodRepository.GetAllAsync();

                if (!methods.Any())
                {
                    return ApiResponse<List<MethodResponseDto>>.FailResponse("Get All Method Fail", "Method are Empty");
                }

                var result = methods.Select(MapToDTO).ToList();

                return ApiResponse<List<MethodResponseDto>>.SuccessResponse(result, "Get All Method Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<MethodResponseDto>>.FailResponse("Get All Method Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var existMethod = await _methodRepository.GetByIdAsync(id);

                if (existMethod == null)
                {
                    return ApiResponse<MethodResponseDto>.FailResponse("Get Method By Id Fail", "Method Are Not Exist");
                }

                var result = MapToDTO(existMethod);

                return ApiResponse<MethodResponseDto>.SuccessResponse(result, "Get Method By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodResponseDto>.FailResponse("Get Method By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodResponseDto>> CreateNewMethodAsync(MethodRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<MethodResponseDto>.FailResponse("Create New Method Fail", "Method is Null");
                }
                var newMethod = MapToEntity(request);

                await _methodRepository.AddAsync(newMethod);

                var result = MapToDTO(newMethod);

                return ApiResponse<MethodResponseDto>.SuccessResponse(result, "Create New Method Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodResponseDto>.FailResponse("Create New Method Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodResponseDto>> UpdateMethodByIdAsync(Guid id, MethodRequestDto newMethod)
        {
            try
            {
                var existMethod = await _methodRepository.GetByIdAsync(id);

                if (existMethod == null)
                {
                    return ApiResponse<MethodResponseDto>.FailResponse("Update Method By Id Fail", "Method Are Not Exist");
                }

                if (newMethod == null)
                {
                    return ApiResponse<MethodResponseDto>.FailResponse("Update Method By Id Fail", "Method is Null");
                }

                existMethod.Name = newMethod.Name;
                existMethod.Description = newMethod.Description;
                existMethod.Difficulty = newMethod.Difficulty;
                existMethod.TimeInfo = newMethod.TimeInfo;

                await _methodRepository.UpdateAsync(existMethod);

                var result = MapToDTO(existMethod);
                return ApiResponse<MethodResponseDto>.SuccessResponse(result, "Update Method Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodResponseDto>.FailResponse("Update Method By Id Fail", ex.Message);
            }
        }


        public async Task<ApiResponse<MethodResponseDto>> DeleteMethodByIdAsync(Guid id)
        {
            try
            {
                var existMethod = await _methodRepository.GetByIdAsync(id);
                if (existMethod == null)
                {
                    return ApiResponse<MethodResponseDto>.FailResponse("Delete Method Fail", "Method Are not exist");
                }

                var result = MapToDTO(existMethod);

                await _methodRepository.DeleteAsync(existMethod);

                return ApiResponse<MethodResponseDto>.SuccessResponse(result, "Delete Method By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodResponseDto>.FailResponse("Delete Method Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<List<MethodSuggestionRespone>>> GetTop3MetodForUserAsync(Guid userId)
        {
            try
            {
                var userAnswersResponse = await _userAnswerService.GetAllUserAnswerByUserIdAsync(userId);

                if (!userAnswersResponse.Success || userAnswersResponse.Data == null || !userAnswersResponse.Data.Any())
                {
                    return ApiResponse<List<MethodSuggestionRespone>>.FailResponse(
                        "User doesn't have any answers",
                        "User has not answered any evaluation questions yet"
                    );
                }

                var userAnswers = userAnswersResponse.Data;

                var choiceIds = userAnswers.Select(ua => ua.ChoiceId).Distinct().ToList();


                var allConditions = new List<MethodRuleConditionResponseDto>();

                foreach (var choiceId in choiceIds)
                {
                    var methodRuleCondtion = await _methodRuleConditionService.GetMethodRuleConditionByChoiceId(choiceId);

                    if (methodRuleCondtion != null)
                    {
                        allConditions.Add(methodRuleCondtion);
                    }
                }

                if (!allConditions.Any())
                {
                    return ApiResponse<List<MethodSuggestionRespone>>.FailResponse(
                        "Get Top 3 Method Fail",
                        "No method rule condition found for user's choices"
                    );
                }

                    var userScores = allConditions
                .Where(c => c.MethodRules != null && c.MethodRules.Any())
                .SelectMany(c => c.MethodRules.Select(mr => new
                {
                    MethodId = mr.MethodId,
                    Weight = c.Weight,
                    Effectiveness = c.Effectiveness
                }))
                .GroupBy(x => x.MethodId)
                .Select(g => new
                {
                    MethodId = g.Key,
                    TotalWeight = g.Sum(x => x.Weight),
                    TotalEffectiveness = g.Sum(x => x.Effectiveness)
                })
                .ToList();

                var allMethodConditionsResponse = await _methodRuleConditionService.GetAllAsync();

                if (!allMethodConditionsResponse.Success || allMethodConditionsResponse.Data == null)
                {
                    return ApiResponse<List<MethodSuggestionRespone>>.FailResponse("Failed to get method conditions","No have Any MethodCondtion");
                }

                var allMethodConditions = allMethodConditionsResponse.Data;

                var methodTotals = allMethodConditions
                    .Where(c => c.MethodRules != null && c.MethodRules.Any())
                    .SelectMany(c => c.MethodRules.Select(mr => new
                    {
                        MethodId = mr.MethodId,
                        Weight = c.Weight,
                        Effectiveness = c.Effectiveness
                    }))
                    .GroupBy(x => x.MethodId)
                    .Select(g => new
                    {
                        MethodId = g.Key,
                        TotalWeight = g.Sum(x => x.Weight),
                        TotalEffectiveness = g.Sum(x => x.Effectiveness)
                    })
                    .ToList();

                var results = new List<(Guid MethodId, double WeightPercent, double EffectPercent, double FinalScore)>();

                foreach (var userMethod in userScores)
                {
                    var total = methodTotals.FirstOrDefault(m => m.MethodId == userMethod.MethodId);

                    if (total == null)
                    {
                        Console.WriteLine($" Không tìm thấy total cho MethodId: {userMethod.MethodId}");
                        continue;
                    }

                    if (total.TotalWeight == 0 || total.TotalEffectiveness == 0)
                    {
                        Console.WriteLine($" Method {userMethod.MethodId} có TotalWeight={total.TotalWeight}, TotalEffectiveness={total.TotalEffectiveness}");
                        continue;
                    }

                    double weightPercent = userMethod.TotalWeight / total.TotalWeight;
                    double effectPercent = userMethod.TotalEffectiveness / total.TotalEffectiveness;
                    double finalScore = ((weightPercent + effectPercent) / 2) * 100;

                    Console.WriteLine($" Method {userMethod.MethodId} -> Weight {userMethod.TotalWeight}/{total.TotalWeight}, Effect {userMethod.TotalEffectiveness}/{total.TotalEffectiveness}");

                    results.Add((userMethod.MethodId, weightPercent * 100, effectPercent * 100, finalScore));
                }



                var top3 = results
                   .OrderByDescending(r => r.FinalScore)
                   .Take(3)
                   .ToList();


                if (!top3.Any())
                {
                    return ApiResponse<List<MethodSuggestionRespone>>.FailResponse(
                        "No top methods found",
                        "Cannot calculate scores for any methods"
                    );
                }

                var methods = await _methodRepository.GetAllAsync();
                var top3Result = new List<MethodSuggestionRespone>();

                foreach (var r in top3)
                {
                    var m = methods.FirstOrDefault(x => x.Id == r.MethodId);
                    if (m == null) continue;

                    var dto = new MethodSuggestionRespone
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Description = m.Description,
                        Difficulty = m.Difficulty,
                        TimeInfo = m.TimeInfo,
                        Pros = m.MethodPros?.Select(p => _proSerivce.MapToDTO(p)).ToList() ?? new List<MethodProResponseDto>(),
                        Cons = m.MethodCons?.Select(c => _conService.MapToDTO(c)).ToList() ?? new List<MethodConResponseDto>(),
                        Techniques = m.Techniques?.Select(t => _techniqueService.MapToDTO(t)).ToList() ?? new List<TechniqueResponseDto>(),
                        MethodRules = m.MethodRules?.Select(r => _methodRuleService.MapToDTO(r)).ToList() ?? new List<MethodRuleResponseDto>(),

                      
                        WeightPercent = Math.Round(r.WeightPercent, 2),
                        EffectivenessPercent = Math.Round(r.EffectPercent, 2),
                        FinalScore = Math.Round(r.FinalScore, 2)
                    };

                    top3Result.Add(dto);
                }

                return ApiResponse<List<MethodSuggestionRespone>>.SuccessResponse(
                    top3Result,
                    "Top 3 methods calculated successfully"
                );

            }
            catch (Exception ex)
            {
                return ApiResponse<List<MethodSuggestionRespone>>.FailResponse("Get Top 3 Method failed", ex.Message);
            }
        }


        protected Method MapToEntity(MethodRequestDto request)
        {
            return new Method
            {
                Name = request.Name,
                Description = request.Description,
                Difficulty = request.Difficulty,
                TimeInfo = request.TimeInfo,
            };
        }

        protected MethodResponseDto MapToDTO(Method method)
        {
            return new MethodResponseDto
            {
                Id = method.Id,
                Name = method.Name,
                Description = method.Description,
                Difficulty = method.Difficulty,
                TimeInfo = method.TimeInfo,
                Pros = method.MethodPros?.Select(p => _proSerivce.MapToDTO(p)).ToList() ?? new List<MethodProResponseDto>(),
                Cons = method.MethodCons?.Select(c => _conService.MapToDTO(c)).ToList() ?? new List<MethodConResponseDto>(),
                Techniques = method.Techniques?.Select(t => _techniqueService.MapToDTO(t)).ToList() ?? new List<TechniqueResponseDto>(),
                MethodRules = method.MethodRules?.Select(mr => _methodRuleService.MapToDTO(mr)).ToList() ?? new List<MethodRuleResponseDto>(),
            };

        }
    }
}
