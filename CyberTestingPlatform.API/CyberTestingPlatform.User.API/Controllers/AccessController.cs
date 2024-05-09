using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.User.API.Models;

namespace CyberTestingPlatform.User.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccessController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAccessControlService _accessControlService;

        public AccessController(
            IAccountService accountService,
            IAccessControlService accessControlService)
        {
            _accountService = accountService;
            _accessControlService = accessControlService;
        }
    }
}