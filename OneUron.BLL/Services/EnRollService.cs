using OneUron.BLL.DTOs.EnRollDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.EnRollRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class EnRollService : IEnRollService
    {
        public readonly IEnRollRepository _enRollRepository;


        public EnRollService(IEnRollRepository enRollRepository)
        {
            _enRollRepository = enRollRepository;
        }

        public async Task<ApiResponse<List<EnRollResponseDto>>> GetAllEnRollAsync()
        {
            try
            {
                var enRolls = await _enRollRepository.GetAllEnRollAsync();

                if (!enRolls.Any())
                {
                    return ApiResponse<List<EnRollResponseDto>>.FailResponse("Get All Fail", "Enroll Are Empty");
                }

                var result = enRolls.Select(MapToDto).ToList();

                return ApiResponse<List<EnRollResponseDto>>.SuccessResponse(result, "Get All Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<List<EnRollResponseDto>>.FailResponse("Get All Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<EnRollResponseDto>> GetEnRollByIdAsync(Guid id)
        {
            try
            {
                var exitEnRoll = await _enRollRepository.GetEnRollByIdAsync(id);

                if (exitEnRoll == null)
                {
                    return ApiResponse<EnRollResponseDto>.FailResponse("Get By Id Fail", "EnRoll Are Not Exists");
                }

                var result = MapToDto(exitEnRoll);

                return ApiResponse<EnRollResponseDto>.SuccessResponse(result, "Get By Id Successfully");

            }
            catch (Exception ex)
            {
                return ApiResponse<EnRollResponseDto>.FailResponse("Get By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<EnRollResponseDto>> CreateNewEnRollAsync(EnRollRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<EnRollResponseDto>.FailResponse("Create New EnRoll Fail", "EnRoll Are Null");
                }

                var newEnRoll = MaptoEnity(request);

                await _enRollRepository.AddAsync(newEnRoll);

                var result = MapToDto(newEnRoll);

                return ApiResponse<EnRollResponseDto>.SuccessResponse(result, "Create New EnRoll Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<EnRollResponseDto>.FailResponse("Create New EnRoll fail", ex.Message);
            }
        }

        public async Task<ApiResponse<EnRollResponseDto>> UpdateEnRollByIdAsync(Guid id, EnRollRequestDto enRollRequestDto)
        {
            try
            {
                var existEnRoll = await _enRollRepository.GetEnRollByIdAsync(id);

                if (existEnRoll == null)
                {
                    return ApiResponse<EnRollResponseDto>.FailResponse("Update EnRoll Fail", "EnRoll Are Not Exists");
                }

                if (enRollRequestDto == null)
                {
                    return ApiResponse<EnRollResponseDto>.FailResponse("Update EnRoll Fail", "EnRoll Are Empty");
                }

                existEnRoll.UserId = enRollRequestDto.UserId;
                existEnRoll.ResourceId = enRollRequestDto.ResourceId;
                existEnRoll.EnrollDate = enRollRequestDto.EnrollDate;

                await _enRollRepository.UpdateAsync(existEnRoll);

                var result = MapToDto(existEnRoll);
                return ApiResponse<EnRollResponseDto>.SuccessResponse(result, "Update EnRoll Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<EnRollResponseDto>.FailResponse("Update EnRoll Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<EnRollResponseDto>> DeleteEnRollByIdAsync(Guid id)
        {
            try
            {
                var exitEnRoll = await _enRollRepository.GetEnRollByIdAsync(id);
                if (exitEnRoll == null)
                {
                    return ApiResponse<EnRollResponseDto>.FailResponse("Delete EnRoll Fail", "EnRoll Are Not Exists");
                }
                var result = MapToDto(exitEnRoll);

                await _enRollRepository.DeleteAsync(exitEnRoll);

                return ApiResponse<EnRollResponseDto>.SuccessResponse(result, "Delete EnRoll Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<EnRollResponseDto>.FailResponse("Delete EnRoll Fail", ex.Message);
            }
        }


        protected EnRollResponseDto MapToDto(EnRoll enRoll)
        {
            return new EnRollResponseDto
            {
                Id = enRoll.Id,
                UserId = enRoll.UserId,
                ResourceId = enRoll.ResourceId,
                EnrollDate = enRoll.EnrollDate,
            };
        }

        protected EnRoll MaptoEnity(EnRollRequestDto newEnRoll)
        {
            return new EnRoll
            {
                UserId = newEnRoll.UserId,
                ResourceId = newEnRoll.ResourceId,
                EnrollDate = newEnRoll.EnrollDate,
            };
        }
    }
}
