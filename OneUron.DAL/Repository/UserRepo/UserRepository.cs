using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.UserRepo
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUserNameAndPasswordAsync(string userName, string password)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password && !u.IsDeleted);
        }
    }
}
