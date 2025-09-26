using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.CourseDetailDTOs
{
    public class CourseDetailRequestDto
    {
        public string Duration { get; set; }

        public string Level { get; set; }

        public string Students { get; set; }

        public double Reviews { get; set; }

        public double Price { get; set; }

        public Guid ResourceId { get; set; }
    }
}
