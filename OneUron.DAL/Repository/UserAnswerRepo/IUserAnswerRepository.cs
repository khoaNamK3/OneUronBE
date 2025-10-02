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
        public Task<List<UserAnswer>> GetAllAsync();
        public Task<List<UserAnswer>> GetByListUserAnswerAsync(Guid userId, Guid eluationQuestionId);
        public Task<UserAnswer> GetByIdAsync(Guid id);
        public Task<List<UserAnswer>> GetUserAnswerByEvaluationIdAsync(Guid userId, Guid evaluationId);
        public  Task DeleteRangeAsync(List<UserAnswer> userAnswers);
        public  Task<List<UserAnswer>> GetAllUserAnswerByUserIdAsync(Guid userId);
    }
}
