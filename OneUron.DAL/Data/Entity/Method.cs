using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Method
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public MethodType Difficulty { get; set; }

        public string TimeInfo { get; set; }

        public virtual StudyMethod StudyMethod { get; set; }

        public virtual ICollection<MethodPro> MethodPros { get; set; }

        public virtual ICollection<MethodCon> MethodCons { get; set; }

        public virtual ICollection<Technique> Techniques { get; set; }

        public virtual ICollection<MethodRule> MethodRules { get; set; }

    }

    public enum MethodType
    {
        Easy = 0,
        Medium = 1,
        Hard = 2,
    }
}
