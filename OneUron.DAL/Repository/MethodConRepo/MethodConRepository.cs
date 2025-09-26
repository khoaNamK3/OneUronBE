using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MethodConRepo
{
    public class MethodConRepository : GenericRepository<MethodCon> , IMethodConRepository
    {
        public MethodConRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<MethodCon>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<MethodCon> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(mc => mc.Id == id);
        }

    }
}
