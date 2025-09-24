using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.AuthDTOs
{
    public class RegisterRequestDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        
        // Default to "User" if not specified
        public string Role { get; set; } = "User";
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime Dob { get; set; }
    }
}
