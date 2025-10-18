using OneUron.BLL.DTOs.QuestionDTOs;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.QuizDTOs
{
    public class QuizResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int TotalQuestion { get; set; }

        public TimeOnly Time { get; set; }

        public QuizType Type { get; set; }

        public double TotalPoints { get; set; }

        public double PassScore { get; set; }

        public Guid UserId { get; set; }
        public List<QuestionResponseDto>? Questions { get; set; } = new List<QuestionResponseDto>();
    }
}
