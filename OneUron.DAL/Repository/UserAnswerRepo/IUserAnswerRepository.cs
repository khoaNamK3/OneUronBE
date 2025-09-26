using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.UserAnswerRepo
{
    public interface IUserAnswerRepository : IGenericRepository<UserAnswer>
    {
        public  Task<List<UserAnswer>> GetAllAsync();
        public Task<List<UserAnswer>> GetByListUserAnswerAsync(Guid userId, Guid eluationQuestionId);
        public  Task<UserAnswer> GetByIdAsync(Guid id);
    }
}
