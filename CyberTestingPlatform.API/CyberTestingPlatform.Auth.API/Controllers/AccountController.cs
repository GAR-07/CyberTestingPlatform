using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using CyberTestingPlatform.Auth.API.Models;
using System.Security.Claims;
using BCrypt.Net;

namespace CyberTestingPlatform.Auth.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly AuthOptions _authOptions;

        public AccountController(
            IConfiguration configuration,
            ApplicationDbContext dbContext)
        {
            _authOptions = configuration.GetSection("AuthOptions").Get<AuthOptions>();
            _dbContext = dbContext;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(model.Password, HashType.SHA512);
                Account? account = await _dbContext.Accounts.FirstOrDefaultAsync(p => p.Email == model.Email);
                if (account != null)
                {
                    if (!BCrypt.Net.BCrypt.EnhancedVerify(model.Password, account.PasswordHash, HashType.SHA512))
                    {
                        return Unauthorized("Password is not valid");
                    }
                    foreach (var role in account.Roles.Split(','))
                        if (role == "Banned")
                        {
                            return Unauthorized("Your account has been blocked");
                        }
                    var response = new
                    {
                        accessToken = GenerateJwt(account),
                    };
                    return Ok(response);
                }
                return Unauthorized("The account does not exist");
            }
            return BadRequest("Invalid model object");
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Role == "Admin" || model.Role == "Moder")
                {
                    return Unauthorized("Incorrect roles");
                }
                var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(model.Password, HashType.SHA512);
                Account? account = await _dbContext.Accounts.FirstOrDefaultAsync(p => p.Email == model.Email);
                if (account == null)
                {
                    var inputDate = model.Birthday.Split('-').Select(Int32.Parse).ToArray();
                    var birthday = new DateTime(inputDate[0], inputDate[1], inputDate[2]);
                    account = new()
                    {
                        Birthday = birthday,
                        Email = model.Email,
                        UserName = model.UserName,
                        Roles = model.Role,
                        PasswordHash = passwordHash,
                    };
                    _dbContext.Accounts.Add(account);
                    await _dbContext.SaveChangesAsync();
                    var response = new
                    {
                        accessToken = GenerateJwt(account),
                    };
                    return Ok(response);
                }
                return Unauthorized("Email already exists");
            }
            return BadRequest("Invalid model object");
        }

        //[HttpGet]
        //[Authorize]
        //[Route("AccountConfirm")]
        //public IActionResult AccountConfirm()
        //{
        //    Guid UserId = Guid.Parse(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value);
        //    Account? account = _dbContext.Accounts.FirstOrDefault(p => p.UserId == UserId);
        //    if (account != null)
        //    {
        //        var response = new
        //        {
        //            userId = account.UserId,
        //            userName = account.UserName,
        //            email = account.Email,
        //            roles = account.Roles.Split(',')
        //        };
        //        return Ok(response);
        //    }
        //    return Unauthorized("The account does not exist");
        //}

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetAllAccounts")]
        public IActionResult GetAllAccounts([FromQuery] ItemsRequestModel model)
        {

            if (int.TryParse(model.SampleSize, out int sampleSize) && int.TryParse(model.Page, out int page))
            {
                try
                {
                    var startItem = _dbContext.Accounts.Select(p => p.UserId).ToList().Count - sampleSize * page;
                    var accountsList = _dbContext.Accounts.Skip(startItem < 0 ? 0 : startItem)
                        .Take(startItem < 0 ? sampleSize + startItem : sampleSize);
                    foreach (var account in accountsList)
                    {
                        account.PasswordHash = "";
                    }
                    return Ok(accountsList);
                }
                catch
                {
                    return BadRequest("Error getting items from the database");
                }
            }
            return BadRequest("Invalid model object");
        }

        private string GenerateJwt(Account account)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, account.UserName),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim(JwtRegisteredClaimNames.Birthdate, account.Birthday.ToString())
            };
            foreach (var role in account.Roles.Split(','))
            {
                claims.Add(new Claim("role", role.ToString()));
            }
            var jwtToken = new JwtSecurityToken(
                issuer: _authOptions.Issuer,    
                audience: _authOptions.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddSeconds(_authOptions.TokenLifeTime),
                signingCredentials: new SigningCredentials(_authOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}