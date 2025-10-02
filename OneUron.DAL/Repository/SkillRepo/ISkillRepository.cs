using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.SkillRepo
{
    public interface ISkillRepository : IGenericRepository<Skill>
    {
        public  Task<List<Skill>> GetAllAsync();

        public  Task<Skill> GetByIdAsync(Guid id);
    }
}
