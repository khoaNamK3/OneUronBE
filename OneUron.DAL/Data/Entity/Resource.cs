using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Resource
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Organization { get; set; }

        public string Description { get; set; }

        public double Star { get; set; }

        public double Reviews { get; set; }

        public double Price { get; set; }

        public ResourceType Type { get; set; }

        public virtual ICollection<EnRoll> EnRolls { get; set; }
    
        public virtual CourseDetail CourseDetail { get; set; }

        public virtual Acknowledge Acknowledge { get; set; }

        public virtual Skill Skills { get; set; }

        public virtual Instructor Instructor { get; set; }
    }

    public enum ResourceType
    {
        New = 0,
        BestSeller = 1,
        EditorChoice = 2,
        Update = 3,
        Certified = 4,
        Tutor = 5,
    }
}
