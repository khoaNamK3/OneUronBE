using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.TechniqueDTOs
{
    public class TechniqueRequestDto
    {
        public string Name { get; set; }

        public Guid MethodId { get; set; }
    }
}
