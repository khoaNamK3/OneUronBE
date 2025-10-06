using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Data.Entity
{
    public class Payment
    {
        public Guid Id { get; set; }

       public DateTime CreateAt { get; set; }

        public double Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        public Guid MemberShipPlanId { get; set; }

        public virtual MemberShipPlan MemberShipPlan { get; set; }
    }

    public enum PaymentStatus
    {
        Pending = 0,       
        Paid = 1,         
        Failed = 2,        
        Cancelled = 3,    
        Refunded = 4,      
        Expired = 5,       
        Processing = 6    
    }
}
