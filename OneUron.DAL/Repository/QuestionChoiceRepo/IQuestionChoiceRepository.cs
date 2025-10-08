using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.QuestionChoiceRepo
{
    public interface IQuestionChoiceRepository : IGenericRepository<QuestionChoice>
    {
        public  Task<List<QuestionChoice>> GetAllQuestionChoiceAsync();

        public  Task<QuestionChoice> GetQuestionChoiceByIdAsync(Guid id);

    }
}
