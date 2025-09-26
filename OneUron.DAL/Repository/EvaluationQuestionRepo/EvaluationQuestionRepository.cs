using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.EvaluationQuestionRepo
{
    public class EvaluationQuestionRepository : GenericRepository<EvaluationQuestion> , IEvaluationQuestionRepository
    {
        public EvaluationQuestionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<EvaluationQuestion>> GetAllAsync()
        {
            return await _dbSet.Include(eq => eq.MethodRuleConditions).Include(eq => eq.Choices).Include(eq => eq.UserAnswers).ToListAsync();
        }

        public async Task<EvaluationQuestion> GetEvaluationQuestionByIdAsync(Guid id)
        {
            return await _dbSet.Include(eq => eq.MethodRuleConditions).Include(eq => eq.Choices).Include(eq => eq.UserAnswers).FirstOrDefaultAsync(eq => eq.Id == id);
        }
    }
}
