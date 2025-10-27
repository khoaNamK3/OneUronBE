using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.UserQuizAttemptDTOs
{
    public class UserQuizInformationResponse
    {
        public int TotalCompleteQuiz {  get; set; }

        public int NumberQuizWaitting { get; set; }

        public int TotalQuizPassed { get; set; }

        public double TotalTime { get; set; }

        public double AverageScore { get; set; }    
    }
}
