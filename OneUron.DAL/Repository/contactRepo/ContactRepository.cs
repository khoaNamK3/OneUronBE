using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.contactRepo
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepository
    {
        public ContactRepository(AppDbContext context) : base(context)
        {
        }


        public async Task<List<Contact>> GetAllContactAsync()
        {
            return await _dbSet.OrderByDescending(c => c.createAt).ToListAsync();
        }
        

        public async Task<Contact> GetContactAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
