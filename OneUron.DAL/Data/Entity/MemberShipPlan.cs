using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class MemberShipPlan
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Fee { get; set; }

        public string Duration { get; set; }

        public virtual ICollection<MemberShip> MemberShips { get; set; }

        public virtual ICollection<Features> Features { get; set; }

        public virtual Payment Payment { get; set; }
    }
}
