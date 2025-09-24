using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.ProfileRepository
{
    public interface IProfileRepository : IGenericRepository<Profile>
    {
        Task<Profile> GetByUserIdAsync(Guid userId);
        Task<Profile> GetProfileWithUserAsync(Guid profileId);
    }
}
