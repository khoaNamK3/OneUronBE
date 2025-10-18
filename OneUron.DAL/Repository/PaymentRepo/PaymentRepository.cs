using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.PaymentRepo
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {

        public PaymentRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Payment>> GetAllPaymentAsync()
        {
            return await _dbSet.Include(p => p.MemberShipPlan).ToListAsync();
        }

        public async Task<Payment> GetPaymentbyIdAsync(Guid id)
        {
            return await _dbSet.Include(p => p.MemberShipPlan).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Payment>> GetPaymentByUserIdAsync(Guid id)
        {
            return await _dbSet.Include(p => p.MemberShipPlan).Where(p => p.UserId == id).ToListAsync();
        }

        public async Task<Payment> GetByOrderCodeAsync(long orderCode)
        {
            return await _dbSet.FirstOrDefaultAsync(p => p.OrderCode == orderCode);
        }
    }
}
