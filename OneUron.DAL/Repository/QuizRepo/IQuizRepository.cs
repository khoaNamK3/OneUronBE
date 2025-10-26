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

        public  Task<List<Quiz>> GetAllQuizByUserId(Guid userId);

        public  Task<PagedResult<Quiz>> GetAllQuizByUserIdAsync(int pageNumber, int pageSize, Guid userId);

        public  Task<PagedResult<Quiz>> GetPagedQuizzesAsync(int pageNumber, int pageSize, string? name);
    }
}
