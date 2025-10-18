using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.PaymentDTOs
{
    public class PaymentRequestDto
    {
        public DateTime CreateAt { get; set; }

        public double Amount { get; set; }

        public PaymentStatus Status { get; set; }

        public Guid UserId { get; set; }

        public long OrderCode { get; set; }
        public Guid MemberShipPlanId { get; set; }
    }
}
