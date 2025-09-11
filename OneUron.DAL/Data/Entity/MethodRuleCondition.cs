using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class MethodRuleCondition
    {

        public Guid Id { get; set; }

        public Guid? EvaluationId { get; set; }

        public virtual Evaluation? Evaluation { get; set; }

        public Guid? EvaluationQuestionId { get; set; }

        public virtual EvaluationQuestion? EvaluationQuestion { get; set; }

        public Guid? ChoiceId { get; set; }
        public virtual Choice? Choice { get; set; }

        public virtual ICollection<MethodRule> MethodRules { get; set; }
    }
}
