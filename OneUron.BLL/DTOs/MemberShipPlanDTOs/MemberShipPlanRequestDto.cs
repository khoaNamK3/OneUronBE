using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.MemberShipPlanDTOs
{
    public class MemberShipPlanRequestDto
    {

        public string Name { get; set; }

        public double Fee { get; set; }

        public string Duration { get; set; }

        public MemberShipPlanType memberShipPlanType { get; set; }

        public List<Guid> FeatureIds { get; set; } = new List<Guid>();
    }
}
