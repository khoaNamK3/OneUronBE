using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.UserRepo
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<User> GetByUserNameAndPasswordAsync(string userName, string password)
        {
            return await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.UserName == userName && u.Password == password && !u.IsDeleted);
        }

        public async Task<User> GetByIdWithRolesAsync(Guid userId)
        {
            return await _context.Set<User>()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
        }

        public async Task<User> GetByUserNameWithRolesAsync(string userName)
        {
            return await _context.Set<User>()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.UserName == userName && !u.IsDeleted);
        }

        public async Task AssignRoleToUserAsync(Guid userId, string roleName)
        {
            var user = await _context.Set<User>()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);

            if (user == null)
                throw new ArgumentException($"User with ID {userId} not found");

            var role = await _context.Set<Role>()
                .FirstOrDefaultAsync(r => r.RoleName == roleName && !r.IsDeleted);

            if (role == null)
                throw new ArgumentException($"Role '{roleName}' not found");

            if (user.Roles == null)
                user.Roles = new List<Role>();

            // Check if user already has this role
            if (!user.Roles.Any(r => r.Id == role.Id))
            {
                user.Roles.Add(role);
                await _context.SaveChangesAsync();
            }
        }
    }
}
