using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MethodRepo
{
    public class MethodRepository : GenericRepository<Method>, IMethodRepository
    {
        public MethodRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Method>> GetAllAsync()
        {
            return await _dbSet.Include(m => m.MethodPros).Include(m => m.MethodCons).Include(m => m.Techniques).Include(m => m.MethodRules).ToListAsync();
        }

        public async Task<Method> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(m => m.MethodPros).Include(m => m.MethodCons).Include(m => m.Techniques).Include(m => m.MethodRules).FirstOrDefaultAsync(m => m.Id == id);
        }

    }
}
