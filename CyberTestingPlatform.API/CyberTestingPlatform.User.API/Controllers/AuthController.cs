using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.User.API.Models;
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.User.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IAccountService _accountService;

        public AuthController(IAuthService authService, IAccountService accountService)
        {
            _authService = authService;
            _accountService = accountService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (ModelState.IsValid)
            {
                var account = await _authService.GetAccountByEmailAsync(model.Email);

                _authService.ValidateAccount(account, model.Password);

                return Ok(new { accessToken = _authService.GenerateJwt(account) });
            }
            return BadRequest("Invalid model object");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (ModelState.IsValid)
            {
                await _authService.ValidateRegistration(model.Email, model.Role);

                var account = new Account(
                    Guid.NewGuid(),
                    _authService.ConvertBirtdayDate(model.Birthday),
                    DateTime.Now,
                    model.Email,
                    model.UserName,
                    _authService.GetPasswordHash(model.Password),
                    model.Role,
                    null);

                await _accountService.CreateAccountAsync(account);

                return Ok(new { accessToken = _authService.GenerateJwt(account) });
            }
            return BadRequest("Invalid model object");
        }
    }
}