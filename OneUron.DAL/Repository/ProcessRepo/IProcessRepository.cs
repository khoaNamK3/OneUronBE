using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.ProcessRepo
{
    public interface IProcessRepository : IGenericRepository<Process>
    {
        public Task<List<Process>> GetAllAsync();

        public Task<Process> GetByIdAsync(Guid id);

        public  Task<List<Process>> GetProcessesByScheduleId(Guid scheduleId);
    }
}
