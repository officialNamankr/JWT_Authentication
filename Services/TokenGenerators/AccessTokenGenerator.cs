using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using JWT_Authentication.Models;

namespace JWT_Authentication.Services.TokenGenerators
{
    public class AccessTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccessTokenGenerator(AuthenticationConfiguration configuration, TokenGenerator tokenGenerator, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {

            List<Claim> claims = new List<Claim>()
            {
                new Claim("id",user.Id.ToString()),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Name,user.UserName)
            };
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return _tokenGenerator.GenerateToken(
                _configuration.AccessTokenSecret,
                _configuration.Issuer,
                _configuration.Audience,
                _configuration.AccessTokenExpirationMinutes,
                claims);


        }
    }
}
