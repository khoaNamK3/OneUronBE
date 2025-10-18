using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.AnswerDTOs
{
    public class AnswerResponseDto
    {
        public Guid Id { get; set; }

        public Guid QuestionId { get; set; }

        public Guid QuestionChoiceId { get; set; }

        public Guid UserQuizAttemptId { get; set; }
    }
}
