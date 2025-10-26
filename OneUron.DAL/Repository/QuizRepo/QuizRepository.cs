using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.QuizRepo
{
    public class QuizRepository : GenericRepository<Quiz>, IQuizRepository
    {
        public QuizRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Quiz>> GetAllQuizAsync()
        {
            return await _dbSet.Include(q => q.UserQuizAttempts).Include(q => q.Questions).ThenInclude(q => q.QuestionChoices).ToListAsync();
        }

        public async Task<Quiz> GetQuizByIdAsync(Guid id)
        {
            return await _dbSet.Include(q => q.UserQuizAttempts).Include(q => q.Questions).ThenInclude(q => q.QuestionChoices).FirstOrDefaultAsync(qu => qu.Id == id);
        }

        public async Task<List<Quiz>> GetAllQuizByUserId(Guid userId)
        {
            return await _dbSet.Include(q => q.UserQuizAttempts).Include(q => q.Questions).ThenInclude(q => q.QuestionChoices).Where(q => q.UserId == userId).ToListAsync();
        }

        public async Task<PagedResult<Quiz>> GetAllQuizByUserIdAsync(int pageNumber, int pageSize, Guid userId)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _dbSet.Where(q => q.UserId == userId).AsNoTracking();

            int totalCount = await query.CountAsync();

            if ((pageNumber - 1) * pageSize >= totalCount && totalCount > 0)
                pageNumber = (int)Math.Ceiling((double)totalCount / pageSize);

            var quizzes = await query
                .OrderByDescending(q => q.PassScore)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Quiz>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = quizzes
            };
        }
        public async Task<PagedResult<Quiz>> GetPagedQuizzesAsync(int pageNumber, int pageSize, string? name)
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


            var quizzes = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Quiz>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = quizzes
            };
        }
    }
}
