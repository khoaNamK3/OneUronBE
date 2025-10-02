using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.MethodRuleDTOs
{
    public class MethodRuleRequestDto
    {
        public Guid MethodId { get; set; }

        public Guid MethodRuleConditionId { get; set; }
    }
}
