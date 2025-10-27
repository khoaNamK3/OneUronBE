using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.AnswerRepo
{
    public interface IAnswerRepository : IGenericRepository<Answer>
    {
        public  Task<List<Answer>> GetAllAnswerAsync();

        public  Task<Answer> GetAnswerByIdAsyc(Guid id);

        public Task CreateListUserAnswerAsync(IEnumerable<Answer> entities);
    }
}
