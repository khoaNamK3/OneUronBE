using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.EnRollRepo
{
    public class EnRollRepository : GenericRepository<EnRoll> , IEnRollRepository
    {
        public EnRollRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<EnRoll>> GetAllEnRollAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<EnRoll> GetEnRollByIdAsync(Guid id)
        {
            return await _dbSet.Include(er => er.User).Include(er => er.Resource).FirstOrDefaultAsync(er => er.Id == id);
        }
    }
}
