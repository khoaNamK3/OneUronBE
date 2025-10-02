using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.MethodRuleDTOs
{
    public class MethodRuleResponseDto
    {
        public Guid Id { get; set; }

        public Guid MethodId { get; set; }

        public Guid MethodRuleConditionId { get; set; }

    }
}
