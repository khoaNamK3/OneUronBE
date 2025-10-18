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
        Task<List<AcknowledgeResponseDto>> GetAllAcknowledgeAsync();
        Task<AcknowledgeResponseDto> GetAcknowledgeByIdAsync(Guid id);
        Task<AcknowledgeResponseDto> CreateNewAcknowledgeAsync(AcknowledgeRequestDto request);
        Task<AcknowledgeResponseDto> UpdateAcknowLedgeByIdAsync(Guid id, AcknowledgeRequestDto newAcknowLedge);
        Task<AcknowledgeResponseDto> DeleteAcknowledgeByIdAsync(Guid id);
        public AcknowledgeResponseDto MapToDTO(Acknowledge acknowledge);
    }
}
