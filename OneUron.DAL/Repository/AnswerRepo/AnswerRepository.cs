using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.AnswerRepo
{
    public class AnswerRepository : GenericRepository<Answer>, IAnswerRepository
    {
        public AnswerRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Answer>> GetAllAnswerAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Answer> GetAnswerByIdAsyc(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(a => a.Id == id);
        }
    
    }
}
