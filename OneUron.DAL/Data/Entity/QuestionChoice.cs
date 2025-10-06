using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class QuestionChoice
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool IsCorrect { get; set; }

        public Guid QuestionId { get; set; }
        public virtual Question Question { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
    }
}
