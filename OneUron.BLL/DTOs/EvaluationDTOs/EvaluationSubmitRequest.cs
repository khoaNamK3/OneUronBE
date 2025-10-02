using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.EvaluationDTOs
{
    public class EvaluationSubmitRequest
    {
        public Guid UserId { get; set; }
        public Guid EvaluationId { get; set; }
        public List<QuestionAnswerDto> Questions { get; set; }
    }

    public class QuestionAnswerDto
    {
        public Guid EvaluationQuestionId { get; set; }  
        public Guid ChoiceId { get; set; }  
    }
}
