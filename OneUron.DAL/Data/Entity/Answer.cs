using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Answer
    {
         public Guid Id { get; set; }

        public Guid QuestionId { get; set; }

        public virtual Question Question { get; set; }

        public Guid UserQuizAttemptId { get; set; }

        public virtual UserQuizAttempt UserQuizAttempt { get; set; }

        public Guid QuestionChoiceId { get; set; }

        public virtual QuestionChoice QuestionChoice { get; set; }

    }
}
