using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.EvaluationQuestionRepo
{
    public interface IEvaluationQuestionRepository : IGenericRepository<EvaluationQuestion>
    {

        public  Task<List<EvaluationQuestion>> GetAllAsync();

        public  Task<EvaluationQuestion> GetEvaluationQuestionByIdAsync(Guid id);

    }
}
