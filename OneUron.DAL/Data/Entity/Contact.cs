using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Contact
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public DateTime createAt { get; set; }

        public string Phone {  get; set; }

        public string Message { get; set; }
    }
}
