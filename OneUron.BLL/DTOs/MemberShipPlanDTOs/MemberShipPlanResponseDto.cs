using OneUron.BLL.DTOs.FeatureDTOs;
using OneUron.BLL.DTOs.MemberShipDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.MemberShipPlanDTOs
{
    public class MemberShipPlanResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public double Fee { get; set; }

        public string Duration { get; set; }

     
        public List<MemberShipResponseDto> MemberShips { get; set; } = new List<MemberShipResponseDto>();

      
        public List<FeatureResponseDto> Features { get; set; } = new List<FeatureResponseDto>();
    }
}
