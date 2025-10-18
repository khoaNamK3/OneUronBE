using FluentValidation;
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
    public class MethodService : IMethodSerivce
    {
        private readonly IMethodRepository _methodRepository;
        private readonly IMethodProSerivce _proService;
        private readonly IMethodConService _conService;
        private readonly ITechniqueService _techniqueService;
        private readonly IMethodRuleService _methodRuleService;
        private readonly IUserAnswerService _userAnswerService;
        private readonly IChoiceService _choiceService;
        private readonly IMethodRuleConditionService _methodRuleConditionService;
        private readonly IValidator<MethodRequestDto> _methodRequestValidator;

        public MethodService(
            IMethodRepository methodRepository,
            IMethodProSerivce proService,
            IMethodConService conService,
            ITechniqueService techniqueService,
            IMethodRuleService methodRuleService,
            IUserAnswerService userAnswerService,
            IChoiceService choiceService,
            IMethodRuleConditionService methodRuleConditionService,
            IValidator<MethodRequestDto> methodRequestValidator)
        {
            _methodRepository = methodRepository;
            _proService = proService;
            _conService = conService;
            _techniqueService = techniqueService;
            _methodRuleService = methodRuleService;
            _userAnswerService = userAnswerService;
            _choiceService = choiceService;
            _methodRuleConditionService = methodRuleConditionService;
            _methodRequestValidator = methodRequestValidator;
        }

     
        public async Task<List<MethodResponseDto>> GetAllAsync()
        {
            var methods = await _methodRepository.GetAllAsync();

            if (methods == null || !methods.Any())
                throw new ApiException.NotFoundException("No methods found.");

            return methods.Select(MapToDTO).ToList();
        }

    
        public async Task<MethodResponseDto> GetByIdAsync(Guid id)
        {
            var method = await _methodRepository.GetByIdAsync(id);
            if (method == null)
                throw new ApiException.NotFoundException($"Method with ID {id} not found.");

            return MapToDTO(method);
        }

        
        public async Task<MethodResponseDto> CreateNewMethodAsync(MethodRequestDto request)
        {
            if (request == null)
                throw new ApiException.BadRequestException("Method request cannot be null.");

            var validationResult = await _methodRequestValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            var newMethod = MapToEntity(request);
            await _methodRepository.AddAsync(newMethod);

            return MapToDTO(newMethod);
        }

        public async Task<MethodResponseDto> UpdateMethodByIdAsync(Guid id, MethodRequestDto newMethod)
        {
            var existMethod = await _methodRepository.GetByIdAsync(id);
            if (existMethod == null)
                throw new ApiException.NotFoundException($"Method with ID {id} not found.");

            if (newMethod == null)
                throw new ApiException.BadRequestException("New method data cannot be null.");

            var validationResult = await _methodRequestValidator.ValidateAsync(newMethod);
            if (!validationResult.IsValid)
                throw new ApiException.ValidationException(validationResult.Errors);

            existMethod.Name = newMethod.Name;
            existMethod.Description = newMethod.Description;
            existMethod.Difficulty = newMethod.Difficulty;
            existMethod.TimeInfo = newMethod.TimeInfo;

            await _methodRepository.UpdateAsync(existMethod);

            return MapToDTO(existMethod);
        }

        
        public async Task<MethodResponseDto> DeleteMethodByIdAsync(Guid id)
        {
            var existMethod = await _methodRepository.GetByIdAsync(id);
            if (existMethod == null)
                throw new ApiException.NotFoundException($"Method with ID {id} not found.");

            await _methodRepository.DeleteAsync(existMethod);
            return MapToDTO(existMethod);
        }

     
        public async Task<List<MethodSuggestionRespone>> GetTop3MethodForUserAsync(Guid userId)
        {
            var userAnswersResponse = await _userAnswerService.GetAllByUserIdAsync(userId);
            if ( userAnswersResponse == null || !userAnswersResponse.Any())
                throw new ApiException.NotFoundException("User has not answered any evaluation questions yet.");

            var userAnswers = userAnswersResponse;
            var choiceIds = userAnswers.Select(ua => ua.ChoiceId).Distinct().ToList();

            var allConditions = new List<MethodRuleConditionResponseDto>();
            foreach (var choiceId in choiceIds)
            {
                var condition = await _methodRuleConditionService.GetMethodRuleConditionByChoiceId(choiceId);
                if (condition != null)
                    allConditions.Add(condition);
            }

            if (!allConditions.Any())
                throw new ApiException.NotFoundException("No method rule condition found for user's choices.");

           
            var userScores = allConditions
                .Where(c => c.MethodRules != null && c.MethodRules.Any())
                .SelectMany(c => c.MethodRules.Select(mr => new
                {
                    mr.MethodId,
                    c.Weight,
                    c.Effectiveness
                }))
                .GroupBy(x => x.MethodId)
                .Select(g => new
                {
                    MethodId = g.Key,
                    TotalWeight = g.Sum(x => x.Weight),
                    TotalEffectiveness = g.Sum(x => x.Effectiveness)
                })
                .ToList();

         
            var allMethodConditions = await _methodRuleConditionService.GetAllAsync();
            if (allMethodConditions == null || !allMethodConditions.Any())
                throw new ApiException.NotFoundException("No method rule conditions found in system.");

            var methodTotals = allMethodConditions
                .Where(c => c.MethodRules != null && c.MethodRules.Any())
                .SelectMany(c => c.MethodRules.Select(mr => new
                {
                    mr.MethodId,
                    c.Weight,
                    c.Effectiveness
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
                if (total == null || total.TotalWeight == 0 || total.TotalEffectiveness == 0)
                    continue;

                double weightPercent = userMethod.TotalWeight / total.TotalWeight;
                double effectPercent = userMethod.TotalEffectiveness / total.TotalEffectiveness;
                double finalScore = ((weightPercent + effectPercent) / 2) * 100;

                results.Add((userMethod.MethodId, weightPercent * 100, effectPercent * 100, finalScore));
            }

            var top3 = results.OrderByDescending(r => r.FinalScore).Take(3).ToList();
            if (!top3.Any())
                throw new ApiException.NotFoundException("No top methods could be calculated.");

            var methods = await _methodRepository.GetAllAsync();
            var top3Result = new List<MethodSuggestionRespone>();

            foreach (var r in top3)
            {
                var m = methods.FirstOrDefault(x => x.Id == r.MethodId);
                if (m == null) continue;

                top3Result.Add(new MethodSuggestionRespone
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    Difficulty = m.Difficulty,
                    TimeInfo = m.TimeInfo,
                    Pros = m.MethodPros?.Select(p => _proService.MapToDTO(p)).ToList() ?? new List<MethodProResponseDto>(),
                    Cons = m.MethodCons?.Select(c => _conService.MapToDTO(c)).ToList() ?? new List<MethodConResponseDto>(),
                    Techniques = m.Techniques?.Select(t => _techniqueService.MapToDTO(t)).ToList() ?? new List<TechniqueResponseDto>(),
                    MethodRules = m.MethodRules?.Select(rm => _methodRuleService.MapToDTO(rm)).ToList() ?? new List<MethodRuleResponseDto>(),
                    WeightPercent = Math.Round(r.WeightPercent, 2),
                    EffectivenessPercent = Math.Round(r.EffectPercent, 2),
                    FinalScore = Math.Round(r.FinalScore, 2)
                });
            }

            return top3Result;
        }

       
        protected Method MapToEntity(MethodRequestDto request)
        {
            return new Method
            {
                Name = request.Name,
                Description = request.Description,
                Difficulty = request.Difficulty,
                TimeInfo = request.TimeInfo
            };
        }

        public MethodResponseDto MapToDTO(Method method)
        {
            if (method == null) return null;

            return new MethodResponseDto
            {
                Id = method.Id,
                Name = method.Name,
                Description = method.Description,
                Difficulty = method.Difficulty,
                TimeInfo = method.TimeInfo,
                Pros = method.MethodPros?.Select(p => _proService.MapToDTO(p)).ToList() ?? new List<MethodProResponseDto>(),
                Cons = method.MethodCons?.Select(c => _conService.MapToDTO(c)).ToList() ?? new List<MethodConResponseDto>(),
                Techniques = method.Techniques?.Select(t => _techniqueService.MapToDTO(t)).ToList() ?? new List<TechniqueResponseDto>(),
                MethodRules = method.MethodRules?.Select(mr => _methodRuleService.MapToDTO(mr)).ToList() ?? new List<MethodRuleResponseDto>()
            };
        }
    }
}
