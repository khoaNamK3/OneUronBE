using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Subject
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public SubjectType Priority { get; set; }

        public Guid ScheduleId { get; set; }

        public virtual Schedule Schedule { get; set; }

        public Guid ProcessId { get; set; }

        public virtual Process Process { get; set; }  
    }

    public enum SubjectType
    {
        High = 0,
        Medium = 1,
        Low = 2,
    }
}
