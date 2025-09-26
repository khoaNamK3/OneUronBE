using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.EvaluationQuestionDTOs
{
    public class EvaluationQuestionRequestDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public EvaluationQuestionType Type { get; set; }

        public Guid EvaluationId { get; set; }
    }
}
