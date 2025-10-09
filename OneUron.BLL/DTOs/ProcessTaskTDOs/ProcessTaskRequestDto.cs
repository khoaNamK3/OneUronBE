using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.ProcessTaskTDOs
{
    public class ProcessTaskRequestDto
    {
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Note { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public bool IsCompleted { get; set; } = false;

        public Guid ProcessId { get; set; }
    }
}
