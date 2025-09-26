using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MethodRulesRepo
{
    public interface IMethodRuleRepository : IGenericRepository<MethodRule>
    {
        public  Task<List<MethodRule>> GetAllAsync();

        public  Task<MethodRule> GetByIdAsync(Guid id);
    }
}
