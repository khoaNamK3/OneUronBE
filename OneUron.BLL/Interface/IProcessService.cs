using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IProcessService
    {
        public Task<ApiResponse<List<ProcessResponseDto>>> GetAllAsync();

        public Task<ApiResponse<ProcessResponseDto>> GetByIdAsync(Guid id);

        public  Task<ApiResponse<ProcessResponseDto>> CreateProcessAsync(ProcessRequestDto request);

        public  Task<ApiResponse<ProcessResponseDto>> UpdateProcessByIdAsync(Guid id, ProcessRequestDto newProcess);

        public  Task<ApiResponse<ProcessResponseDto>> DeleteProcessByIdAsync(Guid id);

        public ProcessResponseDto MapToDTO(Process process);


    }
}
