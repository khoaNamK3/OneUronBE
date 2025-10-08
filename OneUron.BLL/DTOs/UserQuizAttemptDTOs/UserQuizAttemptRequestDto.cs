using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.UserQuizAttemptDTOs
{
    public class UserQuizAttemptRequestDto
    {
        public Guid QuizId { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime FinishAt { get; set; }

        public double Point { get; set; }

        public double Accuracy { get; set; }
    }
}
