using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.contactRepo
{
    public interface IContactRepository : IGenericRepository<Contact>
    {
        public  Task<List<Contact>> GetAllContactAsync();

        public  Task<Contact> GetContactAsync(Guid id);
    }
}
