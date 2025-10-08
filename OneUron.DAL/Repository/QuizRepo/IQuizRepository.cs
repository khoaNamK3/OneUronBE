using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.QuizRepo
{
    public interface IQuizRepository : IGenericRepository<Quiz>
    {
        public  Task<List<Quiz>> GetAllQuizAsync();

        public  Task<Quiz> GetQuizByIdAsync(Guid id);

    }
}
