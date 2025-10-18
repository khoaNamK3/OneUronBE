using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.UserAnswerDTOs
{
    public class UserAnswerResponseDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ChoiceId { get; set; }

        public Guid EvaluationQuestionId { get; set; }
    }
}
