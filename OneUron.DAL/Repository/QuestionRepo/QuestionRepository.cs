using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.QuestionRepo
{
    public class QuestionRepository : GenericRepository<Question> , IQuestionRepository
    {
        public QuestionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Question>> GetAllAsync()
        {
            return await _dbSet.Include(q => q.QuestionChoices).ToListAsync();
        }

        public async Task<Question> GetbyIdAsync(Guid id)
        {
            return await _dbSet.Include(q => q.QuestionChoices).FirstOrDefaultAsync(q => q.Id == id);
        }
    }
}
