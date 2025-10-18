using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.ScheduleRepo
{
    public class ScheduleRepository : GenericRepository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(AppDbContext context) : base(context)
        {
        }


        public async Task<List<Schedule>> GetAllAsync()
        {
            return await _dbSet.Where(c => c.IsDeleted == false).Include(c => c.Processes).ThenInclude(p => p.ProcessTasks).Include(c => c.Subjects).ToListAsync();
        }

        public async Task<Schedule> GetByIdAsync(Guid id)
        {
            return await _dbSet.Where(c => c.IsDeleted == false).Include(c => c.Processes).ThenInclude(p => p.ProcessTasks).Include(c => c.Subjects).FirstOrDefaultAsync(c => c.Id == id);
        }

    }
}
