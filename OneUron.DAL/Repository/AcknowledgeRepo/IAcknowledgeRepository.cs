using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.AcknowledgeRepo
{
    public interface IAcknowledgeRepository :IGenericRepository<Acknowledge>
    {
        public  Task<List<Acknowledge>> GetAllAcknowledgeAsync();

        public  Task<Acknowledge> GetAcknowledgeByIdAsync(Guid id);
    }
}
