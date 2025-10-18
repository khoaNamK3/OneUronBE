using OneUron.BLL.DTOs.TechniqueDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface ITechniqueService
    {
        Task<List<TechniqueResponseDto>> GetAllAsync();
        Task<TechniqueResponseDto> GetByIdAsync(Guid id);
        Task<TechniqueResponseDto> CreateAsync(TechniqueRequestDto request);
        Task<TechniqueResponseDto> UpdateByIdAsync(Guid id, TechniqueRequestDto request);
        Task<TechniqueResponseDto> DeleteByIdAsync(Guid id);
        TechniqueResponseDto MapToDTO(Technique technique);
    }
}
