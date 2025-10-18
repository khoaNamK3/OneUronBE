using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.QuestionChoiceRepo
{
    public class QuestionChoiceRepository : GenericRepository<QuestionChoice> , IQuestionChoiceRepository
    {
        public QuestionChoiceRepository(AppDbContext context) : base(context)
        {
        }


        public async Task<List<QuestionChoice>> GetAllQuestionChoiceAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<QuestionChoice> GetQuestionChoiceByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(qc => qc.Id == id);
        }
    }
}
