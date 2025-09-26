using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.UserAnswerRepo
{
    public class UserAnswerRepository : GenericRepository<UserAnswer>, IUserAnswerRepository
    {
        public UserAnswerRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<UserAnswer>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<List<UserAnswer>> GetByListUserAnswerAsync(Guid userId, Guid eluationQuestionId)
        {
            return await _dbSet.Where(u => u.UserId == userId && u.EvaluationQuestionId == eluationQuestionId).ToListAsync();
        }

        public async Task<UserAnswer> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
