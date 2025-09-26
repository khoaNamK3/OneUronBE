using OneUron.BLL.DTOs.MethodRuleDTOs;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.MethodRuleConditionDTOs
{
    public class MethodRuleConditionResponseDto
    {
        public Guid Id { get; set; }

        public double Weight { get; set; }

        public double Effectiveness { get; set; }

        public Guid? EvaluationId { get; set; }

        public Guid? EvaluationQuestionId { get; set; }

        public Guid? ChoiceId { get; set; }

        public  List<MethodRuleResponseDto> MethodRules { get; set; }
    }
}
