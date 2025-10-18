using OneUron.BLL.DTOs.MethodProDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IMethodProSerivce
    {
        Task<List<MethodProResponseDto>> GetAllAsync();
        Task<MethodProResponseDto> GetByIdAsync(Guid id);
        Task<MethodProResponseDto> CreateNewMethodProAsync(MethodProRequestDto request);
        Task<MethodProResponseDto> UpdateMethodProByIdAsync(Guid id, MethodProRequestDto newMethodPro);
        Task<MethodProResponseDto> DeleteMethodProByIdAsync(Guid id);
        public MethodProResponseDto MapToDTO(MethodPro methodPro);

    }
}
