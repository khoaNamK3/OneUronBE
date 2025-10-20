using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.ScheduleRepo
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        public  Task<List<Schedule>> GetAllAsync();

        public  Task<Schedule> GetByIdAsync(Guid id);

        public  Task<List<Schedule>> GetAllScheduleByUserIdAsync(Guid userId);

    }
}
