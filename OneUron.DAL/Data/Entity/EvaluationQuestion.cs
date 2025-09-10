using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class EvaluationQuestion
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public EvaluationQuestionType Type { get; set; }

        public Guid EvaluationId { get; set; }

        public virtual Evaluation Evaluation { get; set; }

        public virtual ICollection<MethodRuleCondition> MethodRuleConditions { get; set; }

        public virtual ICollection<Choice> Choices { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }
    }

    public enum EvaluationQuestionType
    {
        Multiple = 0,
        Single = 1,
    }

}
