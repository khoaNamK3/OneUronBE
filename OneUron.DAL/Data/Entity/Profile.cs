using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Profile
    {
        public Guid Id { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }

        public string Avatar { get; set; }

        public DateTime Dob {  get; set; }

        public Guid? UserId { get; set; }

        public virtual User User { get; set; }
    }
}
