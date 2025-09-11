using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class MemberShip
    {
        public Guid Id { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime ExpiredDate { get; set; }

        public MemberShipStatus Status { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public Guid MemberShipPlanId { get; set; }

        public virtual MemberShipPlan MemberShipPlan { get; set; }
    }

    public enum MemberShipStatus
    {
        Active = 0,
        Expired = 1,
        Inactive = 2,
    }
}
