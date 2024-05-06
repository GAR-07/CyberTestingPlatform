using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BCrypt.Net;
using CyberTestingPlatform.DataAccess.Repositories;
using CyberTestingPlatform.Core.Models;
using CyberTestingPlatform.Core.Shared;

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

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _accountsRepository.GetAllAsync() ?? throw new CustomHttpException($"Ничего не найдено", 422);
        }

        public async Task<Account> GetAccountDataAsync(Guid userId)
        {
            var account = await _accountsRepository.GetAsync(userId) ?? throw new CustomHttpException($"Аккаунт {userId} не найден", 422);
            account.PasswordHash = null;
            return account;
        }

        public async Task<List<Account>> GetSelectAccountsAsync(int sampleSize, int page)
        {
            if (sampleSize <= 0 || page < 0)
            {
                throw new CustomHttpException($"Заданы невозможные параметры для выборки", 422);
            }

            return await _accountsRepository.GetSelectionAsync(sampleSize, page) ?? throw new CustomHttpException($"Ничего не найдено", 422);
        }

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            return await _accountsRepository.GetByEmailAsync(email) ?? throw new CustomHttpException($"Аккаунт с почтой {email} не найден", 422);
        }

        public async Task<Account> GetAccountAsync(Guid userId)
        {
            return await _accountsRepository.GetAsync(userId) ?? throw new CustomHttpException($"Аккаунт {userId} не найден", 422);
        }

        public async Task<Guid> CreateAccountAsync(Account account)
        {
            return await _accountsRepository.CreateAsync(account) ?? throw new CustomHttpException($"Ошибка создания аккаунта", 422);
        }

        public async Task<Guid> UpdateAccountAsync(Account account)
        {
            return await _accountsRepository.UpdateAsync(account) ?? throw new CustomHttpException($"Аккаунт {account.UserId} не найден", 422);
        }

        public async Task<Guid> DeleteAccountAsync(Guid userId)
        {
            return await _accountsRepository.DeleteAsync(userId) ?? throw new CustomHttpException($"Аккаунт {userId} не найден", 422);
        }

        public bool ValidateAccount(Account account, string password)
        {
            if (account == null)
                throw new CustomHttpException($"Аккаунт {account} не существует", 422);

            if (IsAccountBanned(account.Roles))
                throw new CustomHttpException($"Аккаунт {account} заблокирован", 422);

            if (!IsPasswordValid(password, account.PasswordHash))
                throw new CustomHttpException($"Неверный пароль", 422);

            return true;
        }

        public async Task<bool> ValidateRegistration(string email, string role)
        {
            if (role != "User" && role != "Teacher")
                throw new CustomHttpException($"Задана неверная роль пользователя", 422);

            if (await IsEmailAlreadyExists(email))
                throw new CustomHttpException($"Адрес электронной почты уже занят", 422);

            return true;
        }

        public string GetPasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password, HashType.SHA512);
        }

        public DateTime ConvertBirtdayDate(string birthday)
        {
            var date = birthday.Split('-').Select(Int32.Parse).ToArray();
            return new DateTime(date[0], date[1], date[2]);
        }

        private static bool IsPasswordValid(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash, HashType.SHA512);
        }

        private static bool IsAccountBanned(string roles)
        {
            return roles.Split(',').Contains("Banned");
        }

        private async Task<bool> IsEmailAlreadyExists(string email)
        {
            return await _accountsRepository.GetByEmailAsync(email) != null;
        }
    }
}
