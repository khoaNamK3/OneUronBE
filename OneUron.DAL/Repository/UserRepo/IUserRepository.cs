using OneUron.DAL.Data.Entity;
using OneUron.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace OneUron.DAL.Repository.UserRepo
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUserNameAndPasswordAsync(string userName, string password);
        // Không cần khai báo lại FindAsync vì đã có từ IGenericRepository<User>
    }
}
