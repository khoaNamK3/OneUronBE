using OneUron.BLL.DTOs.ProcessDTOs;
using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.DTOs.SubjectDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.ScheduleDTOs
{
    public class ScheduleResponeDto 
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string TotalTime { get; set; }

        public double AmountSubject { get; set; }

        public DateTime CreateAt { get; set; }

        public bool IsDeleted { get; set; }

        public Guid UserId { get; set; }

        public List<ProcessResponseDto>? Processes { get; set; } = new();
        public List<SubjectResponseDto>? Subjects { get; set; } = new();
    }
}
