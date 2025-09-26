using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.ChoiceRepo
{
    public class ChoiceRepository : GenericRepository<Choice> , IChoiceRepository
    {
        public ChoiceRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Choice>> GetAllAsync()
        {
            return await _dbSet.Include(c => c.MethodRuleConditions).Include(c => c.UserAnswers).ToListAsync();
        }

        public async Task<Choice> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(c => c.MethodRuleConditions).Include(c => c.UserAnswers).FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
