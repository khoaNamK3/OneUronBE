using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.IntructorRepo
{
    public class InstructorRepository : GenericRepository<Instructor>, IInstructorRepository
    {
        public InstructorRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Instructor>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Instructor> GetInstructorByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(i => i.Id == id);
        }
    }
}
