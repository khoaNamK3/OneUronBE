using OneUron.BLL.DTOs.MethodConDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.MethodConRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class MethodConService : IMethodConService
    {
        private readonly IMethodConRepository _methodConRepository;

        public MethodConService(IMethodConRepository methodConRepository)
        {
            _methodConRepository = methodConRepository;
        }

        public async Task<ApiResponse<List<MethodConResponseDto>>> GetAllAsync()
        {
            try
            {
                var methodCons = await _methodConRepository.GetAllAsync();

                if (!methodCons.Any())
                {
                    return ApiResponse<List<MethodConResponseDto>>.FailResponse("Get All Method Con Fail", "Method Con Are Empty");
                }

                var results = methodCons.Select(MapToDTO).ToList();

                return ApiResponse<List<MethodConResponseDto>>.SuccessResponse(results, "Get All Method Con Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<MethodConResponseDto>>.FailResponse("Get All Method Con Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodConResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var existMethodCon = await _methodConRepository.GetByIdAsync(id);

                if (existMethodCon == null)
                {
                    return ApiResponse<MethodConResponseDto>.FailResponse("Get Method Con By Id Fail", "Method Con Are Not Exist");
                }

                var result = MapToDTO(existMethodCon);

                return ApiResponse<MethodConResponseDto>.SuccessResponse(result, "Get Method Con By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodConResponseDto>.FailResponse("Get Method Con By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodConResponseDto>> CreateNewMethodConAsync(MethodConRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<MethodConResponseDto>.FailResponse("Create New Method Con Fail", "Method Con is Null");
                }

                var newMethodCon = MapToEntity(request);

                await _methodConRepository.AddAsync(newMethodCon);

                var result = MapToDTO(newMethodCon);

                return ApiResponse<MethodConResponseDto>.SuccessResponse(result, "Create New Method Con Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<MethodConResponseDto>.FailResponse("Create New Method Con Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodConResponseDto>> UpdateMethodConByIdAsync(Guid id, MethodConRequestDto newMethodCon)
        {
            try
            {
                var existMethodCon = await _methodConRepository.GetByIdAsync(id);

                if (existMethodCon == null)
                {
                    return ApiResponse<MethodConResponseDto>.FailResponse("Update Method Con By Id Fail", "Method Con Are Not Exist");
                }
                if (newMethodCon == null)
                {
                    return ApiResponse<MethodConResponseDto>.FailResponse("Update Method Con By Id Fail", "New Method Con Is Empty");
                }
                existMethodCon.Con = newMethodCon.Con;
                existMethodCon.MethodId = newMethodCon.MethodId;

                await _methodConRepository.UpdateAsync(existMethodCon);

                var result = MapToDTO(existMethodCon);

                return ApiResponse<MethodConResponseDto>.SuccessResponse(result, "Update Method Con By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodConResponseDto>.FailResponse("Update Method Con By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodConResponseDto>> DeleteMethodConByIdAsync(Guid id)
        {
            try
            {
                var existMethodCon = await _methodConRepository.GetByIdAsync(id);

                if (existMethodCon == null)
                {
                    return ApiResponse<MethodConResponseDto>.FailResponse("Delete Method Con By Id Fail", "Method Con Are Not Exist");
                }

                var result = MapToDTO(existMethodCon);

                await _methodConRepository.DeleteAsync(existMethodCon);

                return ApiResponse<MethodConResponseDto>.SuccessResponse(result, "Delete Method Con By Id Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<MethodConResponseDto>.FailResponse("Delete Method Con By Id Fail", ex.Message);
            }
        }

        protected MethodCon MapToEntity(MethodConRequestDto request)
        {
            return new MethodCon
            {
                Con = request.Con,
                MethodId = request.MethodId,
            };
        }

        public MethodConResponseDto MapToDTO(MethodCon method)
        {
            return new MethodConResponseDto
            {
                Id = method.Id,
                Con = method.Con,
                MethodId = method.MethodId
            };
        }
    }
}
