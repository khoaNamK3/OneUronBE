using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MemberShipRepo
{
    public class MemberShipRepository : GenericRepository<MemberShip> , IMemberShipRepository
    {
        public MemberShipRepository(AppDbContext context) : base(context)
        {
        }


        public async Task<List<MemberShip>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<MemberShip> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(ms => ms.MemberShipPlanId == id);
        }
        public async Task<MemberShip> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet.FirstOrDefaultAsync(ms => ms.UserId == userId);
        }
    }
}
