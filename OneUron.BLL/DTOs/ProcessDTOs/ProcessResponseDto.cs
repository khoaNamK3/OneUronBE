using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.DTOs.SubjectDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.ProcessDTOs
{
    public class ProcessResponseDto
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public Guid ScheduleId { get; set; }

        // list Subjects
        public List<SubjectResponseDto>? Subjects { get; set; } = new();

        // list ProcessTasks
        public List<ProcessTaskResponseDto>? ProcessTasks { get; set; } = new();
    }
}
