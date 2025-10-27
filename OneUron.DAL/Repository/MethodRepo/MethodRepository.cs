using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.MethodRepo
{
    public class MethodRepository : GenericRepository<Method>, IMethodRepository
    {
        public MethodRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<Method>> GetAllAsync()
        {
            return await _dbSet.Include(m => m.MethodPros).Include(m => m.MethodCons).Include(m => m.Techniques).ToListAsync();
        }

        public async Task<Method> GetByIdAsync(Guid id)
        {
            return await _dbSet.Include(m => m.MethodPros).Include(m => m.MethodCons).Include(m => m.Techniques).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<PagedResult<Method>> GetMethodPagingAsync(int pageNumber, int pageSize, string? name)
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;

            var query = _dbSet.AsNoTracking().AsQueryable();


            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(q => q.Name.Contains(name));
            }


            int totalCount = await query.CountAsync();


            if ((pageNumber - 1) * pageSize >= totalCount && totalCount > 0)
            {
                pageNumber = (int)Math.Ceiling((double)totalCount / pageSize);
            }

            var method = await query
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

            return new PagedResult<Method>
            {
                CurrentPage = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                Items = method
            };
        }
    }
}
