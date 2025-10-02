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
        public Task<ApiResponse<List<MethodConResponseDto>>> GetAllAsync();

        public Task<ApiResponse<MethodConResponseDto>> GetByIdAsync(Guid id);

        public  Task<ApiResponse<MethodConResponseDto>> CreateNewMethodConAsync(MethodConRequestDto request);

        public  Task<ApiResponse<MethodConResponseDto>> UpdateMethodConByIdAsync(Guid id, MethodConRequestDto newMethodCon);

        public  Task<ApiResponse<MethodConResponseDto>> DeleteMethodConByIdAsync(Guid id);

        public MethodConResponseDto MapToDTO(MethodCon method);
    }
}
