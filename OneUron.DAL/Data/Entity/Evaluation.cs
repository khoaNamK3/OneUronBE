using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Evaluation
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<MethodRuleCondition> MethodRuleConditions { get; set; }

        public virtual ICollection<EvaluationQuestion>? EvaluationQuestions { get; set; }
    }
}
