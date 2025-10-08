using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.QuestionChoiceDTOs
{
    public class QuestionChoiceRequestDto
    {
        public string Name { get; set; }

        public bool IsCorrect { get; set; }

        public Guid QuestionId { get; set; }
    }
}
