using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.ScheduleDTOs
{
    public class ScheduleWeekRespone
    {
        public int TaskOnDay {  get; set; }

        public int TotalTaskOfWeek { get; set; }

        public double TotalTimeOfDay { get; set; }

        public double PercentComplete { get; set; }

    }
}
