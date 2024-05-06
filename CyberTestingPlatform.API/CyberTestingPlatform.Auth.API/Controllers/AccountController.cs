using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.Auth.API.Models;
using CyberTestingPlatform.Core.Models;
using BCrypt.Net;

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

        [HttpPost("Login")]
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

        [HttpPost("Register")]
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

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAccounts")]
        public async Task<IActionResult> GetAccounts([FromQuery] ItemsRequest model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.GetSelectAccountsAsync(model.SampleSize, model.Page);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [Authorize]
        [HttpGet("GetAccountData/{id}")]
        public async Task<IActionResult> GetAccountData(Guid id)
        {
            var response = await _accountService.GetAccountDataAsync(id);
            return Ok(response);
        }
    }
}