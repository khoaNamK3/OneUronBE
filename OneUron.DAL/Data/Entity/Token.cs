using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Token
    {
        public Guid Id { get; set; }

        public string RefreshToken { get; set; }

        public Guid UserId { get; set; }

        public  virtual User User { get; set; }
    }
}
