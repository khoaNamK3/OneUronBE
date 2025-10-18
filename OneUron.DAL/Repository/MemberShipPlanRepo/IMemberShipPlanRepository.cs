using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MemberShipPlanRepo
{
    public interface IMemberShipPlanRepository : IGenericRepository<MemberShipPlan>
    {
        public  Task<List<MemberShipPlan>> GetAllMembertShipPlanAsync();

        public  Task<MemberShipPlan> GetMemberShipPlanByIdAsync(Guid id);
    }
}
