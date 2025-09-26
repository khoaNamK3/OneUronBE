using OneUron.BLL.DTOs.EvaluationQuestionDTOs;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.EvaluationDTOs
{
    public class EvaluationResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }

        public List<MethodRuleConditionResponseDto> MethodRuleConditions { get; set; }

        public List<EvaluationQuestionResponseDto>? EvaluationQuestions { get; set; }
    }
}
