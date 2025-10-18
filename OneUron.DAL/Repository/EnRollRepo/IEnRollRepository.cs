using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.EnRollRepo
{
    public interface IEnRollRepository : IGenericRepository<EnRoll>
    {
        public  Task<List<EnRoll>> GetAllEnRollAsync();

        public  Task<EnRoll> GetEnRollByIdAsync(Guid id);

    }
}
