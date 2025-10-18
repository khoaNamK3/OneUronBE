using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.ProcessTaskTDOs
{
    public class ProcessTaskGenerateRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public double Amount { get; set; }

    }
}
