using OneUron.BLL.DTOs.AdminDTOs;
using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.Interface
{
    public interface IAdminService
    {
        public  Task<AdminResponseDto> GetAdminInforAsync();

        public Task<PagedResult<UserPagingResponseDto>> GetUserPagingAsync(int pageNumber, int pageSize, string userName);
    }
}
