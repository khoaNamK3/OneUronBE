using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.ProcessTaskRepo
{
    public class ProcessTaskRepository : GenericRepository<ProcessTask> , IProcessTaskRepository
    {
        public ProcessTaskRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<ProcessTask>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<ProcessTask> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(pt => pt.Id == id);
        }

        public async Task<List<ProcessTask>> GetAllProcessTaskByProcessIdAsync(Guid processId)
        {
            return await _dbSet.Where(pk => pk.ProcessId == processId).ToListAsync();
        }
    }
}
