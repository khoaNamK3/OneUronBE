using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.PaymentRepo
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        public  Task<List<Payment>> GetAllPaymentAsync();

        public  Task<Payment> GetPaymentbyIdAsync(Guid id);

        public Task<List<Payment>> GetPaymentByUserIdAsync(Guid id);

        public  Task<Payment> GetByOrderCodeAsync(long orderCode);

    }
}
