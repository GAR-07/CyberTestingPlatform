using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Auth.API.Models;
using BCrypt.Net;
using CyberTestingPlatform.Core.Abstractions;
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Auth.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var account = await _accountService.GetAccountByEmail(model.Email);

                var validateError = _accountService.ValidateAccount(account, model.Password);
                if (!string.IsNullOrEmpty(validateError))
                {
                    return BadRequest(validateError);
                }

                var response = new
                {
                    accessToken = _accountService.GenerateJwt(account),
                };
                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var validateError = await _accountService.ValidateRegistration(model.Email, model.Role);
                if (!string.IsNullOrEmpty(validateError))
                {
                    return BadRequest(validateError);
                }

                var passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(model.Password, HashType.SHA512);
                var inputDate = model.Birthday.Split('-').Select(Int32.Parse).ToArray();
                var birthday = new DateTime(inputDate[0], inputDate[1], inputDate[2]);

                var (account, accountError) = Account.Create(
                    Guid.NewGuid(),
                    birthday,
                    model.Email,
                    model.UserName,
                    passwordHash,
                    model.Role);

                if (!string.IsNullOrEmpty(accountError))
                {
                    return BadRequest(accountError);
                }

                await _accountService.CreateAccount(account);

                var response = new
                {
                    accessToken = _accountService.GenerateJwt(account),
                };
                return Ok(response);

            }
            return BadRequest("Invalid model object");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetAccounts")]
        public async Task<IActionResult> GetAccounts([FromQuery] ItemsViewModel model)
        {
            var accounts = await _accountService.GetSelectAccounts(model.SampleSize, model.Page);

            return Ok(accounts);
        }

        //[HttpGet]
        //[Route("GetAllAccounts")]
        //public async Task<IActionResult> GetAllAccounts()
        //{
        //    var accounts = await _accountService.GetAllAccounts();
        //    var response = accounts.Select(x => new AccountResponse(x.UserId, x.Birthday, x.Email, x.UserName, x.Roles));
        //    return Ok(response);
        //}

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
    }
}