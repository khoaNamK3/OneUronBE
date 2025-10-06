using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class UserQuizAttempt
    {
        public Guid Id { get; set; }
        
        public Guid QuizId { get; set; }

        public virtual Quiz Quiz { get; set; }

        public DateTime StartAt { get; set; }

        public  DateTime FinishAt { get; set; }

        public double Point {  get; set; }

        public double Accuracy { get; set; }
    
        public virtual ICollection<Answer> Answers { get; set; }
    }
}
