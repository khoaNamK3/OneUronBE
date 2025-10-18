using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.MemberShipDTOs
{
    public class MemberShipRequestDto
    {
        public DateTime StartDate { get; set; }

        public DateTime ExpiredDate { get; set; }

        public MemberShipStatus Status { get; set; }

        public Guid UserId { get; set; }

        public Guid MemberShipPlanId { get; set; }
    }
}
