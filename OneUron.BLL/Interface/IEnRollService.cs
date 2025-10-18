using OneUron.BLL.DTOs.EnRollDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IEnRollService
    {
        Task<List<EnRollResponseDto>> GetAllEnRollAsync();
        Task<EnRollResponseDto> GetEnRollByIdAsync(Guid id);
        Task<EnRollResponseDto> CreateNewEnRollAsync(EnRollRequestDto request);
        Task<EnRollResponseDto> UpdateEnRollByIdAsync(Guid id, EnRollRequestDto enRollRequestDto);
        Task<EnRollResponseDto> DeleteEnRollByIdAsync(Guid id);

        public EnRollResponseDto MapToDto(EnRoll enRoll);
    }
}
