using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.BLL.DTOs.PaymentDTOs
{
    public  class UpdatePaymentRequestDto
    {
        public PaymentStatus Status { get; set; }

        public long OrderCode { get; set; }
    }
}
