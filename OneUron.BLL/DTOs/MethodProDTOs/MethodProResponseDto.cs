using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.MethodProDTOs
{
    public class MethodProResponseDto
    {
        public Guid Id { get; set; }

        public string Pro { get; set; }

        public Guid MethodId { get; set; }
    }
}
