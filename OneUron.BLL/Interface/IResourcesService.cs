using OneUron.BLL.DTOs.ResourceDTOs;
using OneUron.BLL.ExceptionHandle;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IResourcesService
    {
        Task<List<ResourceResponseDto>> GetAllResourceAsync();
        Task<ResourceResponseDto> GetResourceByIdAsync(Guid id);
        Task<ResourceResponseDto> CreateNewResourceAsync(ResourceRequestDto request);
        Task<ResourceResponseDto> UpdateResourceByIdAsync(Guid id, ResourceRequestDto request);
        Task<ResourceResponseDto> DeleteResourceByIdAsync(Guid id);
        ResourceResponseDto MapToDto(Resource resource);
    }
}
