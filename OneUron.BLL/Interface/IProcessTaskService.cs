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
        Task<List<ProcessTaskResponseDto>> GetAllAsync();
        Task<ProcessTaskResponseDto> GetByIdAsync(Guid id);
        Task<ProcessTaskResponseDto> CreateProcessTaskAsync(ProcessTaskRequestDto request);
        Task<ProcessTaskResponseDto> UpdateProcessTaskByIdAsync(Guid id, ProcessTaskRequestDto request);
        Task<ProcessTaskResponseDto> DeleteProcessTaskByIdAsync(Guid id);
        ProcessTaskResponseDto MapToDTO(ProcessTask processTask);
        Task<ProcessTaskResponseDto> CompleteProcessTaskAsync(Guid processTaskId);
    }
}
