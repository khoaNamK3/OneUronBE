using Microsoft.EntityFrameworkCore;
using OneUron.DAL.Data.Entity;
using System;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.TokenRepo
{
    public class TokenRepository : GenericRepository<Token>, ITokenRepository
    {
        public TokenRepository(AppDbContext context) : base(context) { }

        public async Task<Token> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.RefeshToken == refreshToken);
        }

        public async Task<Token> GetByUserIdAsync(Guid userId)
        {
            return await _dbSet
                .FirstOrDefaultAsync(t => t.UserId == userId);
        }
    }
}