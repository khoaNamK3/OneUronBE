using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MemberShipRepo
{
    public interface IMemberShipRepository : IGenericRepository<MemberShip>
    {
        public  Task<List<MemberShip>> GetAllAsync();

        public  Task<MemberShip> GetByIdAsync(Guid id);

        public  Task<MemberShip> GetByUserIdAsync(Guid userId);

    }
}
