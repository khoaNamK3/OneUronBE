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
        public  Task<ApiResponse<List<ResourceResponseDto>>> GetAllResourceAsync();

        public Task<ApiResponse<ResourceResponseDto>> GetResourceByIdAsync(Guid id);

        public  Task<ApiResponse<ResourceResponseDto>> CreateNewResourceAsync(ResourceRequestDto request);

        public  Task<ApiResponse<ResourceResponseDto>> UpdateResourceByIdAsync(Guid id, ResourceRequestDto request);

        public  Task<ApiResponse<ResourceResponseDto>> DeletedResourceAsync(Guid id);
    }
}
