using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.ProcessRepo
{
    public class ProcessRepository : GenericRepository<Process>, IProcessRepository
    {
        public ProcessRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Process>> GetAllAsync()
        {
            return await _dbSet.Include(p => p.ProcessTasks).Include(p => p.Subjects).ToListAsync();
        }

        public async Task<Process> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(p => p.ProcessTasks).Include(p => p.Subjects).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Process>> GetProcessesByScheduleId(Guid scheduleId)
        {
            return await _dbSet.Include(p => p.ProcessTasks).Include(p => p.Subjects).Where(p => p.ScheduleId == scheduleId).ToListAsync();
        }

    }
}
