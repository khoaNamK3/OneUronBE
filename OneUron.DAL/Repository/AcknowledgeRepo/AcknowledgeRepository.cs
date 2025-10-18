using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.AcknowledgeRepo
{
    public class AcknowledgeRepository : GenericRepository<Acknowledge>, IAcknowledgeRepository
    {
        public AcknowledgeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Acknowledge>> GetAllAcknowledgeAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Acknowledge> GetAcknowledgeByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(al => al.Id == id);
        }

    }
}
