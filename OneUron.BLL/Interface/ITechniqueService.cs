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
        public Task<ApiResponse<List<TechniqueResponseDto>>> GetAllAsync();

        public Task<ApiResponse<TechniqueResponseDto>> GetByIdAsync(Guid id);

        public  Task<ApiResponse<TechniqueResponseDto>> CreateNewTechiqueAsync(TechniqueRequestDto request);

        public  Task<ApiResponse<TechniqueResponseDto>> UpdateTechniqueByIdAsync(Guid id, TechniqueRequestDto newTechnique);

        public  Task<ApiResponse<TechniqueResponseDto>> DeleteTechniqueByidAsync(Guid id);

        public TechniqueResponseDto MapToDTO(Technique technique);
    }
}
