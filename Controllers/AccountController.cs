using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using JWT_Authentication.Models.Dto;
using JWT_Authentication.Services.TokenGenerators;
using JWT_Authentication.Repository.IRepository;
using JWT_Authentication.Helper;
using JWT_Authentication.Services.TokenValidators;
using JWT_Authentication.Models;
using JWT_Authentication.DbContexts;
using JWT_Authentication.Models.Response;

namespace JWT_Authentication.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        ResponseDto _response;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly RefreshTokenValidator _refreshTokenValidator;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly AccessTokenGenerator _accessTokenGenerator;
        private readonly RefreshTokenGenerator _refreshTokenGenerator;

        public AccountController(ApplicationDbContext db, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager,
             RefreshTokenValidator refreshTokenValidator,
            IRefreshTokenRepository refreshTokenRepository,
            AccessTokenGenerator accessTokenGenerator,
            RefreshTokenGenerator refreshTokenGenerator)
        {
            _db = db;
            _response = new ResponseDto();
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _refreshTokenValidator = refreshTokenValidator;
            _refreshTokenRepository = refreshTokenRepository;
            _accessTokenGenerator = accessTokenGenerator;
            _refreshTokenGenerator = refreshTokenGenerator;
        }



        [HttpGet]
        [Route("createInitialRoles")]
        public async Task<object> CreateInitialRoles()
        {
            try
            {
                if (!_roleManager.RoleExistsAsync(Helper.Helper.Role1).GetAwaiter().GetResult())
                {
                    await _roleManager.CreateAsync(new IdentityRole(Helper.Helper.Role1));
                    await _roleManager.CreateAsync(new IdentityRole(Helper.Helper.Role2));
                    await _roleManager.CreateAsync(new IdentityRole(Helper.Helper.Role3));
                }
                _response.Result = Ok();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }

        [HttpPost]
        [Route("Register")]
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "<RoleName>")]
        public async Task<object> Register([FromBody] RegisterViewDto model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = model.UserName,
                        Email = model.Email,
                        Name = model.Name,

                    };
                    var IsUserNamePresent = await _db.Users
                        .AnyAsync(u => u.UserName == user.UserName);
                    var IsEmailPresent = await _db.Users.AnyAsync(u => u.Email == user.Email);
                    if (IsUserNamePresent)
                    {
                        _response.Result = BadRequest();
                        _response.DisplayMessage = "UserName Already Present";
                    }
                    else if (IsEmailPresent)
                    {
                        _response.Result = BadRequest();
                        _response.DisplayMessage = "Email Already Present";
                    }
                    else
                    {
                        var result = await _userManager.CreateAsync(user, model.Password);

                        if (result.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, model.RoleName);
                            await _db.SaveChangesAsync();
                            _response.Result = Ok();
                        }
                    }

                }
                else
                {
                    _response.IsSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
        }



        [HttpPost]
        [Route("LoginJWT")]
        public async Task<object> Login2([FromBody] LoginViewDto model)
        {

            if (!ModelState.IsValid)
            {
                _response.IsSuccess = false;
                _response.Result = BadRequest();
                _response.DisplayMessage = "Invalid model state";
                return _response;
            }
            else
            {
                ApplicationUser user = await _db.Users.Where(u => u.Email.Equals(model.Email)).FirstOrDefaultAsync();
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.Result = Unauthorized();
                    _response.DisplayMessage = "User Does not Exist";
                    return _response;
                }
                var isCorrectPassword = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!isCorrectPassword)
                {
                    _response.IsSuccess = false;
                    _response.Result = Unauthorized();
                    _response.DisplayMessage = "Password Incorrect";
                    return _response;
                }

                var accessToken = _accessTokenGenerator.GenerateToken(user);
                string refreshToken = _refreshTokenGenerator.GenerateToken();

                RefreshToken newRefreshToken = new RefreshToken()
                {
                    Token = refreshToken,
                    UserId = user.Id,
                };
                await _refreshTokenRepository.Create(newRefreshToken);
                _response.Result = Ok(new AuthenticatedUserResponse()
                {
                    AccessToken = accessToken.Result,
                    RefreshToken = refreshToken,
                });

                _response.DisplayMessage = "Logged In successfully";
                return _response;

            }

        }


        [HttpDelete]
        [Route("Logout")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<object> Logout()
        {
            var userId = User.FindFirstValue("id"); //gives the useriD of the logged in User
            if (userId == null)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "first login in to access";
                _response.Result = Unauthorized();
                return _response;
            }
            await _refreshTokenRepository.DeleteAll(userId);//deletes the refresh tokens of the user to logout

            _response.IsSuccess = true;
            _response.DisplayMessage = "Logged out Successfully";
            _response.Result = NoContent();

            return _response;
        }
    }
}
