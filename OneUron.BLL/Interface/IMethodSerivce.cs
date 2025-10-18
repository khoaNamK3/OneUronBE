using OneUron.BLL.DTOs.MethodDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IMethodSerivce
    {
        Task<List<MethodResponseDto>> GetAllAsync();
        Task<MethodResponseDto> GetByIdAsync(Guid id);
        Task<MethodResponseDto> CreateNewMethodAsync(MethodRequestDto request);
        Task<MethodResponseDto> UpdateMethodByIdAsync(Guid id, MethodRequestDto newMethod);
        Task<MethodResponseDto> DeleteMethodByIdAsync(Guid id);
        Task<List<MethodSuggestionRespone>> GetTop3MethodForUserAsync(Guid userId);
    }
}
