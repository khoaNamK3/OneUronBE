using OneUron.BLL.DTOs.AnswerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.UserQuizAttemptDTOs
{
    public class UserQuizAttemptResponseDto
    {

        public Guid Id { get; set; }

        public Guid QuizId { get; set; }

        public DateTime StartAt { get; set; }

        public DateTime FinishAt { get; set; }

        public double Point { get; set; }

        public double Accuracy { get; set; }

        public int TotalQuestion { get; set; }

        public string QuizName { get; set; }

        public bool IsPassed { get; set; }

        public double TotalPoints { get; set; }

        public double PassScore { get; set; }

        public List<AnswerResponseDto>? Answers { get; set; } = new List<AnswerResponseDto>();
    }
}
