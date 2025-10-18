using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.UserQuizAttemptRepo
{
    public class UserQuizAttemptReposiotry : GenericRepository<UserQuizAttempt>, IUserQuizAttemptReposiotry
    {
        public UserQuizAttemptReposiotry(AppDbContext context) : base(context)
        {
        }

        public async Task<List<UserQuizAttempt>> GetAllUserQuizAttemptAsync()
        {
            return await _dbSet.Include(ua => ua.Answers).ToListAsync();
        }

        public async Task<UserQuizAttempt> GetUserQuizAttemptsByIdAsync(Guid id)
        {
            return await _dbSet.Include(ua => ua.Answers).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<UserQuizAttempt>> GetAllUserQuizAttemptByQuizIdAsync(Guid quizId)
        {
            return await _dbSet.Where(q => q.QuizId == quizId).ToListAsync();
        }
    }
}
