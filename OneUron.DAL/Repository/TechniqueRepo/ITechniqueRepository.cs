using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.TechniqueRepo
{
    public interface ITechniqueRepository : IGenericRepository<Technique>
    {
        public  Task<List<Technique>> GetAllAsync();

        public  Task<Technique> GetByIdAsync(Guid id);

    }
}
