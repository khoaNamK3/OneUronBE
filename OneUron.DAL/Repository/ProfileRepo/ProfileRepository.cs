using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.ProfileRepository
{
    public class ProfileRepository : GenericRepository<Profile>, IProfileRepository
    {
        public ProfileRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Profile> GetByUserIdAsync(Guid userId)
        {
            return await _context.Set<Profile>()
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId && !p.User.IsDeleted);
        }

        public async Task<Profile> GetProfileWithUserAsync(Guid profileId)
        {
            return await _context.Set<Profile>()
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == profileId && !p.User.IsDeleted);
        }
    }
}
