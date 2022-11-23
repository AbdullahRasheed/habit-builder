using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using net_api.Data;
using net_api.Data.Requests;
using net_api.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace net_api.Controllers
{
    [ApiController]
    [Route("auth")]
    public class LoginController : ControllerBase
    {

        private readonly LoginService _loginService;
        private readonly UserService _userService;
        private readonly IOptions<JwtConfiguration> _jwtSettings;

        public LoginController(LoginService loginService, UserService userService, IOptions<JwtConfiguration> jwtSettings)
        {
            _loginService = loginService;
            _userService = userService;
            _jwtSettings = jwtSettings;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser([FromBody] LoginCredentialsRequest credentials)
        {
            UserLogin? login = await _loginService.FindByUsernameAsync(credentials.Username);
            if(login is not null)
            {
                return Conflict("Username already exists");
            }

            byte[] salt;
            byte[] hash = GetHash(credentials.Password, out salt);

            UserLogin newLogin = new UserLogin
            {
                Username = credentials.Username,
                PasswordHash = hash,
                PasswordSalt = salt,
                RefreshToken = GenerateRefreshToken()
            };

            await _loginService.InsertAsync(newLogin);
            await _userService.InsertAsync(new User
            {
                Id = newLogin.Id,
                Username = newLogin.Username,
                Habits = new()
            });

            Response.Cookies.Append("refreshToken", newLogin.RefreshToken.Token, new CookieOptions
            {
                Expires = newLogin.RefreshToken.Expires,
                HttpOnly = true,
                IsEssential = true,
                SameSite = SameSiteMode.None,
                Secure = true
            });

            return Ok(GenerateAuthToken(newLogin));
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser([FromBody] LoginCredentialsRequest credentials)
        {
            UserLogin? login = await _loginService.FindByUsernameAsync(credentials.Username);
            if(login is not null)
            {
                if(Verify(credentials.Password, login.PasswordHash, login.PasswordSalt))
                {
                    Response.Cookies.Append("refreshToken", login.RefreshToken.Token, new CookieOptions
                    {
                        Expires = login.RefreshToken.Expires,
                        HttpOnly = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.None,
                        Secure = true
                    });

                    return Ok(GenerateAuthToken(login));
                }
            }

            return Conflict("Incorrect username or password!");
        }

        [HttpPost("refreshauth")]
        public async Task<ActionResult> RefreshAuthToken(string token)
        {
            UserLogin? login = await _loginService.FindByRefreshTokenAsync(token);
            if(login is not null)
            {
                return Ok(GenerateAuthToken(login));
            }

            return Unauthorized();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private string GenerateAuthToken(UserLogin login)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, login.Username),
                new Claim(ClaimTypes.NameIdentifier, login.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Value.SecurityKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private RefreshToken GenerateRefreshToken()
        {
            return new()
            {
                Token = ObjectId.GenerateNewId().ToString(),
                Expires = DateTime.Now.AddDays(7)
            };
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private byte[] GetHash(string t, out byte[] salt)
        {
            using (var func = new HMACSHA512())
            {
                salt = func.Key;
                return func.ComputeHash(Encoding.UTF8.GetBytes(t));
            }
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private bool Verify(string t, byte[] hash, byte[] salt)
        {
            using (var func = new HMACSHA512(salt))
            {
                return hash.SequenceEqual(func.ComputeHash(Encoding.UTF8.GetBytes(t)));
            }
        }
    }
}
