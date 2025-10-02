using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.EvaluationRepo
{
    public interface IEvaluationRepository : IGenericRepository<Evaluation>
    {
        public  Task<List<Evaluation>> GetAllAsync();

        public  Task<Evaluation> GetbyIdAsync(Guid id);

    }
}
