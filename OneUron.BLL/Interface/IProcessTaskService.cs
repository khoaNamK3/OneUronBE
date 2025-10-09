using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IProcessTaskService
    {
        public Task<ApiResponse<List<ProcessTaskResponseDto>>> GetAllAsync();

        public Task<ApiResponse<ProcessTaskResponseDto>> GetByIdAsync(Guid id);

        public Task<ApiResponse<ProcessTaskResponseDto>> CreateProcessTaskAsync(ProcessTaskRequestDto request);

        public Task<ApiResponse<ProcessTaskResponseDto>> UpdateProcessTaskByIdAsync(Guid id, ProcessTaskRequestDto newProcessTask);

        public Task<ApiResponse<ProcessTaskResponseDto>> DeleteProcessTaskByIdAsync(Guid id);

        public ProcessTaskResponseDto MapToDTO(ProcessTask processTask);
    }
}
