using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.CourseDetailRepo
{
    public class CourseDetailRepository : GenericRepository<CourseDetail>, ICourseDetailRepository
    {
        public CourseDetailRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<CourseDetail>> GetAllCourseDetailAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<CourseDetail> GetCourseDetailbyIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
