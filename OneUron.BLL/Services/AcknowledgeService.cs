using OneUron.BLL.DTOs.AcknowledgeDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.BLL.Interface;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository.AcknowledgeRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Services
{
    public class AcknowledgeService : IAcknowledgeService
    {
        private readonly IAcknowledgeRepository _acknowledgeRepository;

        public AcknowledgeService(IAcknowledgeRepository acknowledgeRepository)
        {
            _acknowledgeRepository = acknowledgeRepository;
        }

        public async Task<ApiResponse<List<AcknowledgeResponseDto>>> GetAllAcknowledgeAsync()
        {
            try
            {
                var acknowLedges = await _acknowledgeRepository.GetAllAcknowledgeAsync();

                if (!acknowLedges.Any())
                {
                    return ApiResponse<List<AcknowledgeResponseDto>>.FailResponse("Get All AcKnowledge Fail", "Knowledge Are Empty");
                }

                var result = acknowLedges.Select(MapToDTO).ToList();
                return ApiResponse<List<AcknowledgeResponseDto>>.SuccessResponse(result, "Get Acknowledge Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<AcknowledgeResponseDto>>.FailResponse("Get All Knowledge Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<AcknowledgeResponseDto>> GetAcknowledgeByIdAsync(Guid id)
        {
            try
            {
                var existAcknowLedge = await _acknowledgeRepository.GetAcknowledgeByIdAsync(id);

                if (existAcknowLedge == null)
                {
                    return ApiResponse<AcknowledgeResponseDto>.FailResponse("Get Acknowledge By Id Fail", "Acknowledge Are Not Exist");
                }

                var result = MapToDTO(existAcknowLedge);

                return ApiResponse<AcknowledgeResponseDto>.SuccessResponse(result, "Get Acknowledge By Id Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AcknowledgeResponseDto>.FailResponse("Get Acknowledge By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<AcknowledgeResponseDto>> CreateNewAcknowledgeAsync(AcknowledgeRequestDto request)
        {
            try
            {
                if (request == null)
                {
                    return ApiResponse<AcknowledgeResponseDto>.FailResponse("Create New Acknowledge Fail", "AcknowLedge Are Empty");
                }

                var newAcknowLedge = MapToEnitity(request);
                await _acknowledgeRepository.AddAsync(newAcknowLedge);
                var result = MapToDTO(newAcknowLedge);
                return ApiResponse<AcknowledgeResponseDto>.SuccessResponse(result, "Create new AcknowLedge Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AcknowledgeResponseDto>.FailResponse("Create New Acknowledge Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<AcknowledgeResponseDto>> UpdateAcknowLedgeByIdAsync(Guid id, AcknowledgeRequestDto newAcknowLedge)
        {
            try
            {
                var existAcknowledge = await _acknowledgeRepository.GetAcknowledgeByIdAsync(id);

                if (existAcknowledge == null)
                {
                    return ApiResponse<AcknowledgeResponseDto>.FailResponse("Update Acknowledge By Id Fail", "Acknowledge Are Not Exist");
                }

                if (newAcknowLedge == null)
                {
                    return ApiResponse<AcknowledgeResponseDto>.FailResponse("Update Acknowledge By Id Fail", "Acknowledge Are Emptty");
                }

                existAcknowledge.Text = newAcknowLedge.Text;
                existAcknowledge.CourseId = newAcknowLedge.CourseId;

                await _acknowledgeRepository.UpdateAsync(existAcknowledge);

                var result = MapToDTO(existAcknowledge);

                return ApiResponse<AcknowledgeResponseDto>.SuccessResponse(result, "Update Acknowledge Successafully");

            }
            catch (Exception ex)
            {
                return ApiResponse<AcknowledgeResponseDto>.FailResponse("Update Acknowledge By Id Fail", ex.Message);
            }
        }

        public async Task<ApiResponse<AcknowledgeResponseDto>> DeleteAcknowledgeByIdAsync(Guid id)
        {
            try
            {
                var existAcknowledge = await _acknowledgeRepository.GetAcknowledgeByIdAsync(id);
                if (existAcknowledge == null)
                {
                    return ApiResponse<AcknowledgeResponseDto>.FailResponse("Delete Acknowledge By Id Fail", "Acknowledge Are Not Exist");
                }
                var result = MapToDTO(existAcknowledge);

                await _acknowledgeRepository.DeleteAsync(existAcknowledge);

                return ApiResponse<AcknowledgeResponseDto>.SuccessResponse(result, "Delete AcknowLedge Successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<AcknowledgeResponseDto>.FailResponse("Delete Acknowledge By Id Fail", ex.Message);
            }
        }

        protected Acknowledge MapToEnitity(AcknowledgeRequestDto request)
        {
            return new Acknowledge
            {
                Text = request.Text,
                CourseId = request.CourseId,
            };
        }

        protected AcknowledgeResponseDto MapToDTO(Acknowledge acknowledge)
        {
            return new AcknowledgeResponseDto
            {
                Id = acknowledge.Id,
                Text = acknowledge.Text,
                CourseId = acknowledge.CourseId,
            };
        }
    }
}
