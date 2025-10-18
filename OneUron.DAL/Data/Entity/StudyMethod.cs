using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class StudyMethod
    {
        public Guid Id { get; set; }

        public DateTime UpdateDate { get; set; }

        public bool IsDeleted { get; set; }

        public  Guid MethodId { get; set; }

        public virtual Method Method { get; set; }

        public Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
