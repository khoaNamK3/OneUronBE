using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.IntructorRepo
{
    public interface IInstructorRepository : IGenericRepository<Instructor>
    {
        public  Task<List<Instructor>> GetAllAsync();

        public  Task<Instructor> GetInstructorByIdAsync(Guid id);
    }
}
