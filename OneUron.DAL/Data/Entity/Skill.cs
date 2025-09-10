using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Skill
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public Guid CourseId { get; set; }

        public virtual Resource Resource { get; set; }
    }
}
