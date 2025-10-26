using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MethodRuleConditionRepo
{
    public class MethodRuleConditionRepository : GenericRepository<MethodRuleCondition>, IMethodRuleConditionRepository
    {
        public MethodRuleConditionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<MethodRuleCondition>> GetAllAsync()
        {
            return await _dbSet.Include(mrc => mrc.MethodRules).ToListAsync();
        }

        public async Task<MethodRuleCondition> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(mrc => mrc.MethodRules).ThenInclude(mrc => mrc.Method).FirstOrDefaultAsync(mrc => mrc.Id == id);
        }

        public async Task<List<MethodRuleCondition>> GetMethodRuleConditionByChoiceId(Guid choiceId)
        {
            return await _dbSet.Include(mrc => mrc.MethodRules).ThenInclude(mrc => mrc.Method).Where(mrc => mrc.ChoiceId == choiceId).ToListAsync();
        }
    }
}
