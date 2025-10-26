using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.ContactDTO
{
    public class ContactRequestDto 
    {
        public string Email { get; set; }

        public string Phone { get; set; }

        public string Message { get; set; }
    }
}
