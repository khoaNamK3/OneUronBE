using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MethodRulesRepo
{
    public class MethodRuleRepository : GenericRepository<MethodRule>, IMethodRuleRepository
    {
        public MethodRuleRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<MethodRule>> GetAllAsync()
        {
            return await _dbSet.Include(mr => mr.MethodRuleCondition).Include(mr => mr.Method).ToListAsync();
        }

        public async Task<MethodRule> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(mr => mr.MethodRuleCondition).Include(mr => mr.Method).FirstOrDefaultAsync(mr => mr.Id == id);
        }
    }
}
