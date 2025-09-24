using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.ProfileDTOs
{
    public class CreateProfileDto
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public DateTime Dob { get; set; }
    }
}
