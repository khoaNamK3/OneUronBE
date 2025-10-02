using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.EnRollDTOs
{
    public class EnRollResponseDto
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ResourceId { get; set; }

        public DateTime EnrollDate { get; set; }
    }
}
