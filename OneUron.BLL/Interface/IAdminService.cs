using OneUron.BLL.DTOs.AdminDTOs;
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

    }
}
