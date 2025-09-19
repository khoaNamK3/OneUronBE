using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.ResourceRepo
{
    public interface IResourcesRepository : IGenericRepository<Resource>
    {
          Task<List<Resource>> GetAllResourceAsync();

          Task<Resource> GetResourceByIdAsync(Guid id);
    }
}
