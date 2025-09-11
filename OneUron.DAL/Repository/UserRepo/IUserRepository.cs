using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.UserRepo
{
    public interface IUserRepository
    {
        Task<User> GetByUserNameAndPasswordAsync(string userName, string password);
    }
}
