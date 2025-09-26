using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MethodRepo
{
    public interface IMethodRepository : IGenericRepository<Method>
    {
        public  Task<List<Method>> GetAllAsync();

        public Task<Method> GetByIdAsync(Guid id);
    }
}
