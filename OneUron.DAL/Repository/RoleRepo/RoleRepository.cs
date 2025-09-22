using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.RoleRepo
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Role> GetByNameAsync(string roleName)
        {
            return await _context.Set<Role>()
                .FirstOrDefaultAsync(r => r.RoleName == roleName && !r.IsDeleted);
        }

        public async Task<Role> EnsureRoleExistsAsync(string roleName)
        {
            var role = await GetByNameAsync(roleName);

            if (role == null)
            {
                role = new Role
                {
                    Id = Guid.NewGuid(),
                    RoleName = roleName,
                    IsDeleted = false
                };
                await _context.Set<Role>().AddAsync(role);
                await _context.SaveChangesAsync();
            }

            return role;
        }
    }
}