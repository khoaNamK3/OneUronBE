using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.SubjectDTOs
{
    public class SubjectRequestDto
    {
        public string Name { get; set; }

        public SubjectType Priority { get; set; }

        public Guid ScheduleId { get; set; }

        public Guid? ProcessId { get; set; }
    }
}
