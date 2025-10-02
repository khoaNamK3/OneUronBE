using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MethodRuleConditionRepo
{
    public interface IMethodRuleConditionRepository : IGenericRepository<MethodRuleCondition>
    {
        public  Task<List<MethodRuleCondition>> GetAllAsync();

        public  Task<MethodRuleCondition> GetByIdAsync(Guid id);

        public  Task<MethodRuleCondition> GetMethodRuleConditionByChoiceId(Guid choiceId);

    }
}
