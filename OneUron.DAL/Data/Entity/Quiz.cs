using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Quiz
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TotalQuestion { get; set; }

        public TimeOnly Time { get; set; }

        public QuizType Type { get; set; }

        public double TotalPoints { get; set; }

        public double PassScore { get; set; }


        public virtual ICollection<UserQuizAttempt>? UserQuizAttempts { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

        public virtual Level Level { get; set; }
    }
    public enum QuizType
    {
        Beginner = 0,
        Intermediate = 1,
        Advanced = 2
    }
}
