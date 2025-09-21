using OneUron.BLL.DTOs.AcknowledgeDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IAcknowledgeService
    {
        public Task<ApiResponse<List<AcknowledgeResponseDto>>> GetAllAcknowledgeAsync();

        public Task<ApiResponse<AcknowledgeResponseDto>> GetAcknowledgeByIdAsync(Guid id);

        public  Task<ApiResponse<AcknowledgeResponseDto>> CreateNewAcknowledgeAsync(AcknowledgeRequestDto request);

        public  Task<ApiResponse<AcknowledgeResponseDto>> UpdateAcknowLedgeByIdAsync(Guid id, AcknowledgeRequestDto newAcknowLedge);

        public  Task<ApiResponse<AcknowledgeResponseDto>> DeleteAcknowledgeByIdAsync(Guid id);
    }
}
