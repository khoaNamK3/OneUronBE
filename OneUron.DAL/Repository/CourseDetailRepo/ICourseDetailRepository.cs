using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.CourseDetailRepo
{
    public interface ICourseDetailRepository :IGenericRepository<CourseDetail>
    {
        public  Task<List<CourseDetail>> GetAllCourseDetailAsync();

        public  Task<CourseDetail> GetCourseDetailbyIdAsync(Guid id);

    }
}
