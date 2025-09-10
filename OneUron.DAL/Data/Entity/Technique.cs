using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Technique
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid MethodId { get; set; }

        public virtual Method Method { get; set; }
    }
}
