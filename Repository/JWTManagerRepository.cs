using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JWT_Authentication.Repository.IRepository;
using JWT_Authentication.Models;
using JWT_Authentication.Models.Dto;
using JWT_Authentication.DbContexts;

namespace JWT_Authentication.Repository
{
    public class JWTManagerRepository : IJWTManagerRepository
    {
        private readonly ApplicationDbContext _db;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration iconfiguration;
        public JWTManagerRepository(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IConfiguration iconfiguration)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            this.iconfiguration = iconfiguration;
        }
        public async Task<Tokens> AuthenticateAsync(LoginViewDto users)
        {
            var signedUser = await _userManager.FindByEmailAsync(users.Email);
            var result = await _userManager.CheckPasswordAsync(signedUser, users.Password);
            if (!result)
            {
                return null;
            }

            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
              {
                    new Claim(ClaimTypes.Email, users.Email)
              }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Tokens { Token = tokenHandler.WriteToken(token) };

        }
    }
}
