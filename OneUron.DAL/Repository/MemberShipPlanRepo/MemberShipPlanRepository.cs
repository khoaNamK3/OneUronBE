using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MemberShipPlanRepo
{
    public class MemberShipPlanRepository : GenericRepository<MemberShipPlan>, IMemberShipPlanRepository
    {
        public MemberShipPlanRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<MemberShipPlan>> GetAllMembertShipPlanAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<MemberShipPlan> GetMemberShipPlanByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
