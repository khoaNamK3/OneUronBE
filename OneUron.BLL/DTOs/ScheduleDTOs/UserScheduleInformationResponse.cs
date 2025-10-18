using OneUron.BLL.DTOs.ProcessTaskTDOs;
using OneUron.BLL.DTOs.SubjectDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.ScheduleDTOs
{
    public class UserScheduleInformationResponse
    {
        public double TotalQuizUserAttemp {  get; set; }

        public double TotalTimeProcessTaskComplete { get; set; }

        public double Streak {  get; set; }

        public List<ProcessTaskResponseDto> NearProcessTaskComplete { get; set; }

        public int NumbetTaskComplete { get; set; }

        public List<SubjectResponseDto> SubjectFuture { get; set; }

    }
}
