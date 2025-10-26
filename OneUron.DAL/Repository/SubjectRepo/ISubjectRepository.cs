using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.SubjectRepo
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        public Task<List<Subject>> GetAllAsync();

        public Task<Subject> GetByIdAsyn(Guid id);

        public  Task<List<Subject>> GetAllSubjectbyScheduleIdAsync(Guid scheduleId);

        public  Task<List<Subject>> GetSubjectByProcessIdAsync(Guid processId);
    }
}
