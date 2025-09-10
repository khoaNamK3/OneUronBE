using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class UserAnswer
    {
        public Guid UserId { get; set; }
        
        public virtual User User { get; set; }
        
        public Guid ChoiceId { get; set; }
        
        public virtual Choice Choice { get; set; }
        
        public Guid EvaluationQuestionId { get; set; }

        public virtual EvaluationQuestion EvaluationQuestion { get; set; }
    }
}
