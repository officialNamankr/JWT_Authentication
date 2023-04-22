using JWT_Authentication.Models;

namespace JWT_Authentication.Repository.IRepository
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken> GetByToken(string refreshToken);
        Task Create(RefreshToken refreshToken);
        Task Delete(Guid id);

        Task DeleteAll(string id);
    }
}
