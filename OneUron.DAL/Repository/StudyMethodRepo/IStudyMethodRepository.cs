using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.StudyMethodRepo
{
    public interface IStudyMethodRepository : IGenericRepository<StudyMethod>
    {
        public  Task<List<StudyMethod>> GetALlAsync();

        public  Task<StudyMethod> GetByIdAsync(Guid id);

        public  Task<StudyMethod> GetStudyMethodByUserIdAsync(Guid userId);
    }
}
