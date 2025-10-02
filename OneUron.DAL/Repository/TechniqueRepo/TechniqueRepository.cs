using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.TechniqueRepo
{
    public class TechniqueRepository : GenericRepository<Technique> , ITechniqueRepository
    {
        public TechniqueRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Technique>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }


        public async Task<Technique> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.Id == id);
        }
    }
}