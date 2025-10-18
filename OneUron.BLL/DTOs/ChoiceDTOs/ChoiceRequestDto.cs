using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.ChoiceDTOs
{
    public class ChoiceRequestDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Guid EvaluationQuestionId { get; set; }
    }
}
