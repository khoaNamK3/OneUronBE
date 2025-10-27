using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.AdminDTOs
{
    public class AdminResponseDto
    {
        public int UserCount { get; set; } = 0;

        public double TotalAmount { get; set; } = 0;

        public int PaymentFail { get; set; } = 0;

        public int MemberShip { get; set; } = 0;
    }
}
