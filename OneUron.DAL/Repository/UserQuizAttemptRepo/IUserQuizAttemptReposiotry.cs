using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.UserQuizAttemptRepo
{
    public interface IUserQuizAttemptReposiotry : IGenericRepository<UserQuizAttempt>
    {
        public  Task<List<UserQuizAttempt>> GetAllUserQuizAttemptAsync();

        public  Task<UserQuizAttempt> GetUserQuizAttemptsByIdAsync(Guid id);
    }
}
