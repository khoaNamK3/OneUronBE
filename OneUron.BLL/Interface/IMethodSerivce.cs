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
        public Task<ApiResponse<List<MethodResponseDto>>> GetAllAsync();

        public Task<ApiResponse<MethodResponseDto>> GetByIdAsync(Guid id);

        public  Task<ApiResponse<MethodResponseDto>> CreateNewMethodAsync(MethodRequestDto request);

        public  Task<ApiResponse<MethodResponseDto>> UpdateMethodByIdAsync(Guid id, MethodRequestDto newMethod);

        public  Task<ApiResponse<MethodResponseDto>> DeleteMethodByIdAsync(Guid id);

        public  Task<ApiResponse<List<MethodSuggestionRespone>>> GetTop3MetodForUserAsync(Guid userId);
    }
}
