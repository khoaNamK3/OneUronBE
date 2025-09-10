using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class User
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public DateTime CreatedDate { get; set; }
        
        public DateTime UpdateDate { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Token Token {  get; set; }

        public virtual Profile Profile { get; set; }

        public virtual ICollection<Role> Roles { get; set; } // many to many 

        public virtual ICollection<UserQuizAttempt>? UserQuizAttempts { get; set; }

        public virtual ICollection<EnRoll> EnRolls { get; set; }

        public virtual ICollection<StudyMethod> StudyMethods { get; set; }

        public virtual ICollection<UserAnswer> UserAnswers { get; set; }

        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
