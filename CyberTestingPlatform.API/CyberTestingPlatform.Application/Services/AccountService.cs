using CyberTestingPlatform.Resourse.API.Models;
using CyberTestingPlatform.Core.Abstractions;
using CyberTestingPlatform.Core.Models;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using BCrypt.Net;

// Сервисы соединяют базу данных с контроллерами API
// Контроллер будет вызывать эти сервисы
// Использование репозиториев, с их помощью осущ. валидация, кэширование, обращение к другим базам данных - всё это здесь
// Сейчас методы простые, но логику в контроллерах хранить не хорошо, поэтому эта логика будет тут

namespace CyberTestingPlatform.Application.Services
{
    public class AccountService : IAccountService
    {
        private readonly AuthOptions _authOptions;
        private readonly IAccountsRepository _accountsRepository;

        public AccountService(
            IConfiguration configuration,
            IAccountsRepository accountsRepository)
        {
            _authOptions = configuration.GetSection("AuthOptions").Get<AuthOptions>();
            _accountsRepository = accountsRepository;
        }

        public string GenerateJwt(Account account)
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

        public async Task<List<Account>> GetAllAccounts()
        {
            return await _accountsRepository.GetAll();
        }

        public async Task<List<Account>?> GetSelectAccounts(int sampleSize, int page)
        {
            if (sampleSize > 0 && page > 0)
            {
                return await _accountsRepository.GetSelection(sampleSize, page);
            }

            return null;
        }

        public async Task<Account?> GetAccountByEmail(string email)
        {
            return await _accountsRepository.GetByEmail(email);
        }

        public async Task<Guid> CreateAccount(Account account)
        {
            return await _accountsRepository.Create(account);
        }

        public async Task<Guid> UpdateAccount(Guid userId, DateTime birthday, string email, string userName, string passwordHash, string roles)
        {
            return await _accountsRepository.Update(userId, birthday, email, userName, passwordHash, roles);
        }

        public async Task<Guid> DeleteAccount(Guid userId)
        {
            return await _accountsRepository.Delete(userId);
        }

        public string? ValidateAccount(Account? account, string password)
        {
            if (account == null)
                return "The account does not exist";

            if (!IsPasswordValid(password, account.PasswordHash))
                return "Password is not valid";

            if (IsAccountBanned(account.Roles))
                return "Your account has been blocked";

            return null;
        }

        public async Task<string?> ValidateRegistration(string email, string role)
        {
            if (role == "Admin" || role == "Moder")
                return "Incorrect roles";

            if (await IsEmailAlreadyExists(email))
                return "Email already exists";

            return null;
        }

        public bool IsPasswordValid(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash, HashType.SHA512);
        }

        public bool IsAccountBanned(string roles)
        {
            return roles.Split(',').Contains("Banned");
        }

        public async Task<bool> IsEmailAlreadyExists(string email)
        {
            var existingAccount = await _accountsRepository.GetByEmail(email);
            return existingAccount != null;
        }
    }
}
