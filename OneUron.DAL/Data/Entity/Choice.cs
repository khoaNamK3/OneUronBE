using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Choice
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid EvaluationQuestionId { get; set; }
        public virtual EvaluationQuestion EvaluationQuestion { get; set; }

        public virtual ICollection<MethodRuleCondition> MethodRuleConditions { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }
}
