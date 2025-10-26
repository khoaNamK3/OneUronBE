using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.DTOs.UserAnswerDTOs;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.ChoiceDTOs
{
    public class ChoiceResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public Guid EvaluationQuestionId { get; set; }

        public List<MethodRuleConditionResponseDto> MethodRuleConditions { get; set; }

        //public List<UserAnswerResponseDto> UserAnswers { get; set; }
    }
}
