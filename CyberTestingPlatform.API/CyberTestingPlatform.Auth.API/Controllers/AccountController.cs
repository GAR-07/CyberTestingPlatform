using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.Auth.API.Models;
using CyberTestingPlatform.Core.Models;
using BCrypt.Net;
using CyberTestingPlatform.Resourse.API.Models;

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
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var account = await _accountService.GetAccountByEmailAsync(model.Email);

                _accountService.ValidateAccount(account, model.Password);

                return Ok(new { accessToken = _accountService.GenerateJwt(account) });
            }
            return BadRequest("Invalid model object");
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                await _accountService.ValidateRegistration(model.Email, model.Role);

                var account = new Account(
                    Guid.NewGuid(),
                    _accountService.ConvertBirtdayDate(model.Birthday),
                    model.Email,
                    model.UserName,
                    _accountService.GetPasswordHash(model.Password),
                    model.Role);

                await _accountService.CreateAccountAsync(account);

                return Ok(new { accessToken = _accountService.GenerateJwt(account) });
            }
            return BadRequest("Invalid model object");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("GetAccounts")]
        public async Task<IActionResult> GetAccounts([FromQuery] ItemsRequest model)
        {
            if (ModelState.IsValid)
            {
                var accounts = await _accountService.GetSelectAccountsAsync(model.SampleSize, model.Page);

                return Ok(accounts);
            }
            return BadRequest("Invalid model object");
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