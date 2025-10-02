using OneUron.BLL.DTOs.ChoiceDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IChoiceService
    {
        public Task<ApiResponse<List<ChoiceResponseDto>>> GetAllAsync();

        public Task<ApiResponse<ChoiceResponseDto>> GetByIdAsync(Guid id);

        public  Task<ApiResponse<ChoiceResponseDto>> CreateNewChoiceAsync(ChoiceRequestDto request);

        public  Task<ApiResponse<ChoiceResponseDto>> UpdateChoiceByIdAsync(Guid id, ChoiceRequestDto newChoice);

        public  Task<ApiResponse<ChoiceResponseDto>> DeleteChoiceByIdAsync(Guid id);

        public ChoiceResponseDto MapToDTO(Choice choice);
    }
}
