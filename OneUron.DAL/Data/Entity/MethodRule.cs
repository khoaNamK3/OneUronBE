using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class MethodRule
    {
        public Guid Id { get; set; }
        
        public Guid MethodId { get; set; }

        public virtual Method Method { get; set; }

        public double Weight { get; set; }

        public double Effectiveness { get; set; }

        public virtual ICollection<MethodRuleCondition> MethodRuleConditions { get; set; }
    }
}
