using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.ProcessDTOs
{
    public class ProcessRequestDto
    {
        public DateTime Date { get; set; }

        public string Description { get; set; }

        public Guid? ScheduleId { get; set; }
    }
}
