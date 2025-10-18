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
        Task<List<ProcessResponseDto>> GetAllAsync();
        Task<ProcessResponseDto> GetByIdAsync(Guid id);
        Task<ProcessResponseDto> CreateProcessAsync(ProcessRequestDto request);
        Task<ProcessResponseDto> UpdateProcessByIdAsync(Guid id, ProcessRequestDto newProcess);
        Task<ProcessResponseDto> DeleteProcessByIdAsync(Guid id);
        Task<List<ProcessResponseDto>> GetProcessesByScheduleId(Guid scheduleId);
        public ProcessResponseDto MapToDTO(Process process);


    }
}
