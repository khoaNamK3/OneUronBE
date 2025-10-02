using OneUron.DAL.Data.Entity;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.RoleRepo
{
    public interface IRoleRepository : IGenericRepository<Data.Entity.Role>
    {
        Task<Data.Entity.Role> GetByNameAsync(string roleName);
        Task<Data.Entity.Role> EnsureRoleExistsAsync(string roleName);
    }
}