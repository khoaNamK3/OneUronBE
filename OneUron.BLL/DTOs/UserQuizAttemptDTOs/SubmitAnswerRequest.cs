using OneUron.BLL.DTOs.AnswerDTOs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.UserQuizAttemptDTOs
{
    public class SubmitAnswerRequest
    {
        public Guid QuizId { get; set; }

        public DateTimeOffset? StartTime { get; set; }

        public DateTimeOffset? FinishTime { get; set; }

        public double? Point { get; set; }

        public double? Accuracy { get; set; }

        public List<ListAnswerRequest>? AnswerList { get; set; }
    }

    public class ListAnswerRequest
    {
        public Guid QuestionId { get; set; }

        public Guid QuestionChoiceId { get; set; }
    }

}
