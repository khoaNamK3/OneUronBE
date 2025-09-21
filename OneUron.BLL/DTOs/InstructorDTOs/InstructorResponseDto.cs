using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.InstructorDTOs
{
    public class InstructorResponseDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Experience { get; set; }

        public string Contact { get; set; }

        public Guid CourseId { get; set; }
    }
}
