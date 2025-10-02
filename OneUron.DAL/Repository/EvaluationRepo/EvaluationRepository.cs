using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.EvaluationRepo
{
    public class EvaluationRepository : GenericRepository<Evaluation>, IEvaluationRepository
    {
        public EvaluationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Evaluation>> GetAllAsync()
        {
            return await _dbSet
                    .Include(e => e.EvaluationQuestions)
                    .ThenInclude(eq => eq.Choices)
                    .Where(e => !e.IsDeleted)
                    .ToListAsync();
        }

        public async Task<Evaluation> GetbyIdAsync(Guid id)
        {
            return await _dbSet
                    .Include(e => e.EvaluationQuestions)
                    .ThenInclude(eq => eq.Choices)
                    .FirstOrDefaultAsync(e => e.Id == id && !e.IsDeleted);
        }

    }
}
