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
        public Task<ApiResponse<List<MethodProResponseDto>>> GetALlAsync();

        public Task<ApiResponse<MethodProResponseDto>> GetByIdAsync(Guid id);

        public  Task<ApiResponse<MethodProResponseDto>> CreateNewMethoProAsync(MethodProRequestDto request);

        public  Task<ApiResponse<MethodProResponseDto>> UpdateMethodProByIdAsync(Guid id, MethodProRequestDto newMethodPro);

        public  Task<ApiResponse<MethodProResponseDto>> DeleteMethodProByIdAsync(Guid id);

        public MethodProResponseDto MapToDTO(MethodPro methodPro);

    }
}
