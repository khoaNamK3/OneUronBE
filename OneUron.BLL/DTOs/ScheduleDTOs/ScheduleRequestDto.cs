using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.ScheduleDTOs
{
    public class ScheduleRequestDto
    {
        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string TotalTime { get; set; }

        public double AmountSubject { get; set; }

        public DateTime CreateAt { get; set; }

        public Guid UserId { get; set; }
    }
}
