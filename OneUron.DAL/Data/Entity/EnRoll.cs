using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class EnRoll
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        
        public virtual User User { get; set; }

        public Guid ResourceId { get; set; }

        public virtual Resource Resource { get; set; }

        public DateTime EnrollDate {  get; set; }
    }
}
