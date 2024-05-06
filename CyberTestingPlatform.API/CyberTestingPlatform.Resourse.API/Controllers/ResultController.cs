using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Application.Services;

namespace CyberTestingPlatform.Resourse.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ResultController : Controller
    {
        private readonly ITestResultService _testResultService;

        public ResultController(ITestResultService testResultService)
        {
            _testResultService = testResultService;
        }
    }
}