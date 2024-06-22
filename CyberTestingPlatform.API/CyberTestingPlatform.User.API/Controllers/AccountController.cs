using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.User.API.Models;
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.User.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAccessControlService _accessControlService;

        public AccountController(
            IAccountService accountService,
            IAccessControlService accessControlService)
        {
            _accountService = accountService;
            _accessControlService = accessControlService;
        }

        [Authorize]
        [HttpGet("GetAccount/{id}")]
        public async Task<IActionResult> GetAccount(Guid id)
        {
            if (ModelState.IsValid)
            {
                var canAccess = _accessControlService.CanAccessAccount(id, User);

                if (canAccess)
                {
                    var account = await _accountService.GetAccountAsync(id);
                    return Ok(account);
                }
                else
                {
                    return Forbid();
                }
            }
            return BadRequest("Invalid model object");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAccounts")]
        public async Task<IActionResult> GetAccounts([FromQuery] SearchRequest model)
        {
            if (ModelState.IsValid)
            {
                var accounts = await _accountService.GetAccountsAsync(model.SearchText, model.Page, model.PageSize);

                return Ok(accounts);
            }
            return BadRequest("Invalid model object");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("UpdateRoles/{id}")]
        public async Task<IActionResult> UpdateRoles(Guid id, [FromQuery] string roles)
        {
            if (ModelState.IsValid)
            {
                var account = await _accountService.UpdateRolesAsync(id, roles);

                return Ok(account);
            }
            return BadRequest("Invalid model object");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("BlockAccount/{id}")]
        public async Task<IActionResult> BlockAccount(Guid id)
        {
            if (ModelState.IsValid)
            {
                await _accountService.BlockAccountAsync(id);

                return Ok();
            }
            return BadRequest("Invalid model object");
        }
    }
}