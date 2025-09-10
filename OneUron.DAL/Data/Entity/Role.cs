using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Role
    {
        public Guid Id { get; set; }

        public string RoleName { get; set; }

        public bool IsDeleted { get; set; }

        public virtual ICollection<User> Users { get; set; } // many to many
    }
}
