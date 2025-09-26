using OneUron.BLL.DTOs.MethodProDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.MethodProRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class MethodProSerivce : IMethodProSerivce
    {
        private readonly IMethodProRepository _methodProRepository;

        public MethodProSerivce(IMethodProRepository methodProRepository)
        {
            _methodProRepository = methodProRepository;
        }

        public async Task<ApiResponse<List<MethodProResponseDto>>> GetALlAsync()
        {
            try
            {
                var methodPros = await _methodProRepository.GetAllAsync();

                if (!methodPros.Any())
                {
                    return ApiResponse<List<MethodProResponseDto>>.FailResponse("Get All MethodPro Fail", "Method Pro Is Empty");
                }

                var results = methodPros.Select(MapToDTO).ToList();

                return ApiResponse<List<MethodProResponseDto>>.SuccessResponse(results, "Get All MethodPro Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<MethodProResponseDto>>.FailResponse("Get All MethodPro Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodProResponseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var existMethodPro = await _methodProRepository.GetByIdAsync(id);

                if (existMethodPro == null)
                {
                    return ApiResponse<MethodProResponseDto>.FailResponse("Get MethodPro By Id Fail", "Method Pro Are Not Exist");
                }

                var result = MapToDTO(existMethodPro);

                return ApiResponse<MethodProResponseDto>.SuccessResponse(result, "Get MethodPro By Id Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<MethodProResponseDto>.FailResponse("Get MethodPro By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodProResponseDto>> CreateNewMethoProAsync(MethodProRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<MethodProResponseDto>.FailResponse("Create new MethodPro Fail", "New MethodPro is Null");
                }

                var newMethodPro = MapToEntity(request);

                await _methodProRepository.AddAsync(newMethodPro);

                var result = MapToDTO(newMethodPro);

                return ApiResponse<MethodProResponseDto>.SuccessResponse(result, "Create New MethodPro Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodProResponseDto>.FailResponse("Create new MethodPro Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<MethodProResponseDto>> UpdateMethodProByIdAsync(Guid id, MethodProRequestDto newMethodPro)
        {
            try
            {
                var existMethodPro = await _methodProRepository.GetByIdAsync(id);

                if (existMethodPro == null)
                {
                    return ApiResponse<MethodProResponseDto>.FailResponse("Update MethodPro By Id Fail", "MethodPro is Not Exist");
                }
                if (newMethodPro == null)
                {
                    return ApiResponse<MethodProResponseDto>.FailResponse("Update MethodPro By Id Fail", "New MethodPro is Null");
                }

                existMethodPro.Pro = newMethodPro.Pro;
                existMethodPro.MethodId = newMethodPro.MethodId;

                await _methodProRepository.UpdateAsync(existMethodPro);

                var result = MapToDTO(existMethodPro);

                return ApiResponse<MethodProResponseDto>.SuccessResponse(result, "Update MethodPro By Id Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<MethodProResponseDto>.FailResponse("Update MethodPro By Id Fail", ex.Message);
            }
        }


        public async Task<ApiResponse<MethodProResponseDto>> DeleteMethodProByIdAsync(Guid id)
        {
            try
            {
                var existMethodPro = await _methodProRepository.GetByIdAsync(id);

                if (existMethodPro == null)
                {
                    return ApiResponse<MethodProResponseDto>.FailResponse("Update MethodPro By Id Fail", "MethodPro is Not Exist");
                }

                var result = MapToDTO(existMethodPro);

                await _methodProRepository.DeleteAsync(existMethodPro);

                return ApiResponse<MethodProResponseDto>.SuccessResponse(result, "Delete MethodPro Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<MethodProResponseDto>.FailResponse("Update MethodPro By Id Fail", ex.Message);
            }
        }

        protected MethodPro MapToEntity(MethodProRequestDto newMethodPro)
        {
            return new MethodPro
            {
                Pro = newMethodPro.Pro,
                MethodId = newMethodPro.MethodId,
            };
        }

        public MethodProResponseDto MapToDTO(MethodPro methodPro)
        {
            return new MethodProResponseDto
            {
                Id = methodPro.Id,
                Pro = methodPro.Pro,
                MethodId = methodPro.MethodId,
            };
        }
    }
}
