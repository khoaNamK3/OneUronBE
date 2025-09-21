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
        public Task<ApiResponse<List<EnRollResponseDto>>> GetAllEnRollAsync();

        public Task<ApiResponse<EnRollResponseDto>> GetEnRollByIdAsync(Guid id);

        public Task<ApiResponse<EnRollResponseDto>> CreateNewEnRollAsync(EnRollRequestDto request);

        public Task<ApiResponse<EnRollResponseDto>> UpdateEnRollByIdAsync(Guid id, EnRollRequestDto enRollRequestDto);
    
        public Task<ApiResponse<EnRollResponseDto>> DeleteEnRollByIdAsync(Guid id);
    }
}
