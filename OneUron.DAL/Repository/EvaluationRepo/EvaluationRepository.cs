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

        public async Task<PagedResult<Evaluation>> GetPagingEvalutionAsync(int pageNumber, int pageSize, string? name)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _dbSet.AsNoTracking().AsQueryable();


            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(q => q.Name.Contains(name));
            }

            int totalCount = await query.CountAsync();


            if ((pageNumber - 1) * pageSize >= totalCount && totalCount > 0)
            {
                pageNumber = (int)Math.Ceiling((double)totalCount / pageSize);
            }

            var evaluations = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Evaluation>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = evaluations
            };
        }

    }
}
