using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MethodProRepo
{
    public interface IMethodProRepository : IGenericRepository<MethodPro>
    {
        public  Task<List<MethodPro>> GetAllAsync();

        public  Task<MethodPro> GetByIdAsync(Guid id);

    }
}
