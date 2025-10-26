using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.SubjectRepo
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Subject>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Subject> GetByIdAsyn(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Subject>> GetAllSubjectbyScheduleIdAsync(Guid scheduleId)
        {
            return await _dbSet.Where(s => s.ScheduleId == scheduleId).ToListAsync();
        }

        public async Task<List<Subject>> GetSubjectByProcessIdAsync(Guid processId)
        {
            return await _dbSet.Where(p => p.Processes.Any(p => p.Id == processId)).ToListAsync();
        }

    }
}
