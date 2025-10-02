using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MethodProRepo
{
    public class MethodProRepository : GenericRepository<MethodPro>, IMethodProRepository
    {
        public MethodProRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<MethodPro>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<MethodPro> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(mp => mp.Id == id);
        }


    }
}
