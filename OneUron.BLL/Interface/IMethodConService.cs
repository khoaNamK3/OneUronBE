using OneUron.BLL.DTOs.MethodConDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IMethodConService
    {
        Task<List<MethodConResponseDto>> GetAllAsync();
        Task<MethodConResponseDto> GetByIdAsync(Guid id);
        Task<MethodConResponseDto> CreateNewMethodConAsync(MethodConRequestDto request);
        Task<MethodConResponseDto> UpdateMethodConByIdAsync(Guid id, MethodConRequestDto newMethodCon);
        Task<MethodConResponseDto> DeleteMethodConByIdAsync(Guid id);

        public MethodConResponseDto MapToDTO(MethodCon method);
    }
}
