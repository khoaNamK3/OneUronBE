using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.ProcessTaskRepo
{
    public interface IProcessTaskRepository : IGenericRepository<ProcessTask>
    {
        public  Task<List<ProcessTask>> GetAllAsync();

        public  Task<ProcessTask> GetByIdAsync(Guid id);

        public  Task<List<ProcessTask>> GetAllProcessTaskByProcessIdAsync(Guid processId);
    }
}
