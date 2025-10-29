using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.PaymentDTOs
{
    public class PaymentChartResponse
    {
        public List<MonthlyPaymentSummary> ChartData { get; set; }
        public List<int> Year { get; set; }
    }
}
