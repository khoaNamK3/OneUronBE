using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.ResourceRepo
{
    public class ResourcesRepository : GenericRepository<Resource>, IResourcesRepository
    {
        public ResourcesRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Resource>> GetAllResourceAsync()
        {
            return await _dbSet.Include(r => r.CourseDetail).Include(r => r.Acknowledges).Include(r => r.Skills).Include(r => r.Instructors).ToListAsync();
        }

        public async Task<Resource> GetResourceByIdAsync(Guid id)
        {
            return await _dbSet.Include(r => r.CourseDetail).Include(r => r.Acknowledges).Include(r => r.Skills).Include(r => r.Instructors).FirstOrDefaultAsync(r => r.Id == id);
        }


    }
}
