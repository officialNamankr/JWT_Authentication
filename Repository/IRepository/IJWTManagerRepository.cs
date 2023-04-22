using JWT_Authentication.Models;
using JWT_Authentication.Models.Dto;

namespace JWT_Authentication.Repository.IRepository
{
    public interface IJWTManagerRepository
    {
        Task<Tokens> AuthenticateAsync(LoginViewDto users);
    }
}
