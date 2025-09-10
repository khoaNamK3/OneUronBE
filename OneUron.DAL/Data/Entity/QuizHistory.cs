using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class QuizHistory
    {
        public Guid Id { get; set; }

        public Guid ChoiceId { get; set; }
        public virtual QuestionChoice Choice { get; set; }

        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public Guid UserQuizAttemptId { get; set; }
        public virtual UserQuizAttempt UserQuizAttempts { get; set; }
    }
}
