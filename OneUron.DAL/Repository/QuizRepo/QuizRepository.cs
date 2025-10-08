using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.QuizRepo
{
    public class QuizRepository : GenericRepository<Quiz> , IQuizRepository
    {
        public QuizRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Quiz>> GetAllQuizAsync()
        {
            return await _dbSet.Include(q => q.Questions).ThenInclude(q => q.QuestionChoices).ToListAsync();
        }

        public async Task<Quiz> GetQuizByIdAsync(Guid id)
        {
            return await _dbSet.Include(q => q.Questions).ThenInclude(q => q.QuestionChoices).FirstOrDefaultAsync(qu => qu.Id == id);
        }

    }
}
