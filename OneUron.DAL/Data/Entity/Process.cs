using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Process
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public Guid ScheduleId { get; set; }

        public virtual Schedule Schedule { get; set; }

        public virtual ICollection<Subject>? Subjects { get; set; }

        public virtual ICollection<ProcessTask> ProcessTasks { get; set; }

    }
}
