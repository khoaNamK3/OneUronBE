using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.PaymentDTOs
{
    public class MonthlyPaymentSummary
    {
        public int Month { get; set; }
        public double TotalAmount { get; set; }
    }
}
