using OneUron.BLL.DTOs.ChoiceDTOs;
using OneUron.BLL.DTOs.MethodRuleConditionDTOs;
using OneUron.BLL.DTOs.UserAnswerDTOs;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.EvaluationQuestionDTOs
{
    public class EvaluationQuestionResponseDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public EvaluationQuestionType Type { get; set; }

        public Guid EvaluationId { get; set; }

        public List<ChoiceResponseDto> Choices { get; set; } = new();
        public List<UserAnswerResponseDto> userAnswers { get; set; } = new();
        public List<MethodRuleConditionResponseDto> MethodRuleConditions { get; set; } = new();
    }
}
