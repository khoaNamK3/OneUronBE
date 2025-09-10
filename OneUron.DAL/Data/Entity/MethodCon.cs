using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class MethodCon
    {
        public Guid Id { get; set; }

        public string Con {  get; set; }

        public Guid MethodId { get; set; }

        public virtual Method Method { get; set; }
    }
}
