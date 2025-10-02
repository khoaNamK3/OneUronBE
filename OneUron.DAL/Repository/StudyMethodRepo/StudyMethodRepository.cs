using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.StudyMethodRepo
{
    public class StudyMethodRepository : GenericRepository<StudyMethod> , IStudyMethodRepository
    {
        public StudyMethodRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<StudyMethod>> GetALlAsync()
        {
            return await _dbSet.Include(sm => sm.Method).Where(sm => !sm.IsDeleted).ToListAsync();
        }

        public async Task<StudyMethod> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(sm => sm.Method).FirstOrDefaultAsync(sm => sm.Id == id && !sm.IsDeleted);
        }
    }
}
