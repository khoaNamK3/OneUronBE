using OneUron.BLL.DTOs.QuestionChoiceDTOs;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.QuestionDTOs
{
    public class QuestionResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Point { get; set; }

        public Guid QuizId { get; set; }

        public List<QuestionChoiceReponseDto>? QuestionChoices { get; set; } = new List<QuestionChoiceReponseDto>();
    }
}
