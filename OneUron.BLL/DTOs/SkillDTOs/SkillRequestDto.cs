using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.SkillDTOs
{
    public class SkillRequestDto
    {
        public string Text { get; set; }

        public Guid CourseId { get; set; }
    }
}
