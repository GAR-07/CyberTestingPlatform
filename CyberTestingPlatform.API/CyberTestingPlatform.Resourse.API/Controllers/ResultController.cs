using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.Core.Models;
using CyberTestingPlatform.Resourse.API.Models;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize]
        [HttpGet("GetTestResult/{id}")]
        public async Task<IActionResult> GetTestResult(Guid id)
        {
            if (ModelState.IsValid)
            {
                var testResult = await _testResultService.GetTestResult(id);

                var response = new TestResultsResponse(
                    testResult.Id,
                    testResult.TestId,
                    testResult.UserId,
                    testResult.Answers,
                    testResult.Results,
                    testResult.CreationDate);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [Authorize]
        [HttpGet("GetTestResultsByUser/{id}")]
        public async Task<IActionResult> GetTestResultsByUser([FromQuery] SearchRequest request, Guid id)
        {
            if (ModelState.IsValid)
            {
                var testResult = await _testResultService.GetSelectionTestResultsByUser(request.SearchText, request.Page, request.PageSize, id);

                var response = testResult.Select(x => new TestResultsResponse(
                    x.Id,
                    x.TestId,
                    x.UserId,
                    x.Answers,
                    x.Results,
                    x.CreationDate));

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [Authorize]
        [HttpGet("GetTestResultsByTest/{id}")]
        public async Task<IActionResult> GetTestResultsByTest([FromQuery] SearchRequest request, Guid id)
        {
            if (ModelState.IsValid)
            {
                var testResults = await _testResultService.GetSelectionTestResultsByTest(request.SearchText, request.Page, request.PageSize, id);

                var response = testResults.Select(x => new TestResultsResponse(
                    x.Id,
                    x.TestId,
                    x.UserId,
                    x.Answers,
                    x.Results,
                    x.CreationDate));

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [Authorize]
        [HttpPost("CreateTestResult")]
        public async Task<IActionResult> CreateTestResult([FromBody] TestResultsRequest request)
        {
            if (ModelState.IsValid)
            {
                var testResult = new TestResult(
                    Guid.NewGuid(),
                    Guid.Parse(request.TestId),
                    Guid.Parse(request.UserId),
                    request.Answers,
                    null,
                    DateTime.Now);

                var testResultId = await _testResultService.CreateTestResultAsync(testResult);

                return Ok(testResultId);
            }
            return BadRequest("Invalid model object");
        }
    }
}