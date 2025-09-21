using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.SkillRepo
{
    public class SkillRepository : GenericRepository<Skill>, ISkillRepository
    {
        public SkillRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Skill>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Skill> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Id == id);
        }

    }
}
