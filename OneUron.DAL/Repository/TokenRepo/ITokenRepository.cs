using OneUron.DAL.Data.Entity;
using System;
using System.Threading.Tasks;

namespace OneUron.DAL.Repository.TokenRepo
{
    public interface ITokenRepository : IGenericRepository<Token>
    {
        Task<Token> GetByRefreshTokenAsync(string refreshToken);
        Task<Token> GetByUserIdAsync(Guid userId);
    }
}
