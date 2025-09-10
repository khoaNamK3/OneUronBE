using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Schedule
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string TotalTime { get; set; }

        public double AmountSubject { get; set; }

        public DateTime CreateAt { get; set; }

        public bool IsDeleted { get; set; }

        public Guid UserId { get; set; }
        
        public virtual User User { get; set; }

        public virtual ICollection<Process> Processes { get; set; }

        public virtual ICollection<Subject> Subjects { get; set; }

    }
}
