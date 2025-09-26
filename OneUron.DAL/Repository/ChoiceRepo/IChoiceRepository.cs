using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.ChoiceRepo
{
    public interface IChoiceRepository : IGenericRepository<Choice>
    {
        public  Task<List<Choice>> GetAllAsync();

        public  Task<Choice> GetByIdAsync(Guid id);
    }
}
