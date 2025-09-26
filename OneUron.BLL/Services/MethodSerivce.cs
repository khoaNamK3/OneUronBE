using OneUron.BLL.DTOs.MethodConDTOs;
using OneUron.BLL.DTOs.MethodDTOs;
using OneUron.BLL.DTOs.MethodProDTOs;
using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.BLL.DTOs.TechniqueDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.MethodRepo;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public MethodSerivce(IMethodRepository methodRepository, IMethodProSerivce proSerivce, IMethodConService conService,ITechniqueService techniqueService,IMethodRuleService methodRuleService)
        {
            _methodRepository = methodRepository;
            _proSerivce = proSerivce;
            _conService = conService;
            _techniqueService = techniqueService;
            _methodRuleService = methodRuleService;
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
                return ApiResponse<MethodResponseDto>.FailResponse("Delete Method Fail",ex.Message);
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
