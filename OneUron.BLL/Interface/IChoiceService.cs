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
        Task<List<ChoiceResponseDto>> GetAllAsync();
        Task<ChoiceResponseDto> GetByIdAsync(Guid id);
        Task<ChoiceResponseDto> CreateNewChoiceAsync(ChoiceRequestDto request);
        Task<ChoiceResponseDto> UpdateChoiceByIdAsync(Guid id, ChoiceRequestDto newChoice);
        Task<ChoiceResponseDto> DeleteChoiceByIdAsync(Guid id);

        public ChoiceResponseDto MapToDTO(Choice choice);
    }
}
