using OneUron.DAL.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.UserRepo
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }

        public async Task<User> GetByUserNameAndPasswordAsync(string userName, string password)
        {
            return await _dbSet
                .FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password && !u.IsDeleted);
        }
    }
}
