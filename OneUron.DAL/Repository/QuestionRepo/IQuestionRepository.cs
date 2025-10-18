using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.QuestionRepo
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        public  Task<List<Question>> GetAllAsync();

        public  Task<Question> GetbyIdAsync(Guid id);
    }
}
