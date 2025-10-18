using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.featureRepo
{
    public interface IFeatureRepository : IGenericRepository<Features>
    {
        public  Task<List<Features>> GetAllAsync();

        public  Task<Features> GetByIdAsync(Guid id);


    }
}
