using OneUron.BLL.DTOs.SubjectDTOs;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.ScheduleDTOs
{
    public class ScheduleSubjectRequestDto
    {
        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string TotalTime { get; set; }

        public double AmountSubject { get; set; }

        public DateTime CreateAt { get; set; }

        public List<SubjectListRequest>? subjectListRequest { get; set; }
    }


    public class SubjectListRequest
    {
        public string Name { get; set; }

        public SubjectType Priority { get; set; }
    }
}
