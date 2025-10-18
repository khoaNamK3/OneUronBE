using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.AcknowledgeDTOs
{
    public class AcknowledgeRequestDto
    {
        public string Text { get; set; }

        public Guid CourseId { get; set; }
    }
}
