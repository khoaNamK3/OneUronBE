using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MethodConRepo
{
    public interface IMethodConRepository : IGenericRepository<MethodCon>
    {
        public  Task<List<MethodCon>> GetAllAsync();

        public  Task<MethodCon> GetByIdAsync(Guid id);

    }
}
