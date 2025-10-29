using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public async Task<List<Payment>> GetAllPaymentSucessfullyAsync()
        {
            return await _dbSet.Where(p => p.Status == PaymentStatus.Paid).ToListAsync();   
        }

        public async Task<List<Payment>> GetAllPaymentFaidAsync()
        {
            return await _dbSet.Where(p => p.Status == PaymentStatus.Failed).ToListAsync();
        }

        public async Task<List<Payment>> CalculateTotalPaymentEachMonthOfYearAsync(int year)
        {
            var payments = await _dbSet.Where(p => p.CreateAt.Year == year && p.Status == PaymentStatus.Paid).ToListAsync();

            var result = payments.GroupBy(p => p.CreateAt.Month).Select(
                g => new Payment
                {
                    CreateAt = new DateTime(year, g.Key, 1),
                    Amount = g.Sum(p => p.Amount)
                }).OrderBy(x => x.CreateAt.Month).ToList();
            
            return result;
        }

        public async Task<List<int>> GetAllYearAsync()
        {
            return await _dbSet.Select(p => p.CreateAt.Year).Distinct().OrderByDescending(y => y).ToListAsync();
        }

        public async Task<List<Payment>> RecentPaymentAsync()
        {
            var fromDate = DateTime.UtcNow.AddDays(-7);

            var recentPayments = await _dbSet.Where(p => p.CreateAt >= fromDate && p.CreateAt <= DateTime.UtcNow
                    && p.Status == PaymentStatus.Paid).OrderByDescending(p => p.CreateAt).ToListAsync();

            return recentPayments;
        }
    }
}
