using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Resourse.API.Models;
using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Resourse.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        private readonly ITestService _testService;

        public TestController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpPost("CheckResultTest/{id}")]
        [Authorize]
        public async Task<IActionResult> CheckResultTest(Guid id, [FromBody] TestsRequest request)
        {
            if (ModelState.IsValid)
            {
                var test = await _testService.GetTestAsync(id);
                var response = request.CorrectAnswers.Equals(test.CorrectAnswers, StringComparison.OrdinalIgnoreCase);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [HttpGet("GetCorrectAnswersTest/{id}")]
        [Authorize]
        public async Task<IActionResult> GetCorrectAnswersTest(Guid id)
        {
            if (ModelState.IsValid)
            {
                var test = await _testService.GetTestAsync(id);
                var response = test.CorrectAnswers;

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [HttpGet("GetTestsByCourseId/{id}")]
        [Authorize]
        public async Task<IActionResult> GetTestsByCourseId(Guid id)
        {
            if (ModelState.IsValid)
            {
                var tests = await _testService.GetTestsByCourseIdAsync(id);

                var response = tests.Select(x => new TestsResponse(
                    x.Id,
                    x.Theme,
                    x.Title,
                    x.Questions,
                    x.AnswerOptions,
                    null,
                    x.Position,
                    x.CreatorID,
                    x.CreationDate,
                    x.LastUpdationDate,
                    x.CourseId));

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [HttpGet("GetTest/{id}")]
        [Authorize]
        public async Task<IActionResult> GetTest(Guid id)
        {
            if (ModelState.IsValid)
            {
                var test = await _testService.GetTestAsync(id);

                var response = new TestsResponse(
                    test.Id,
                    test.Theme,
                    test.Title,
                    test.Questions,
                    test.AnswerOptions,
                    null,
                    test.Position,
                    test.CreatorID,
                    test.CreationDate,
                    test.LastUpdationDate,
                    test.CourseId);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [HttpGet]
        [Authorize]
        [Route("GetTests")]
        public async Task<IActionResult> GetTests([FromQuery] ItemsRequest request)
        {
            if (ModelState.IsValid)
            {
                var tests = await _testService.GetSelectTestsAsync(request.SampleSize, request.Page);

                var response = tests.Select(x => new TestsResponse(
                    x.Id,
                    x.Theme,
                    x.Title,
                    x.Questions,
                    x.AnswerOptions,
                    null,
                    x.Position,
                    x.CreatorID,
                    x.CreationDate,
                    x.LastUpdationDate,
                    x.CourseId));

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Creator")]
        [Route("CreateTest")]
        public async Task<IActionResult> CreateTest([FromBody] TestsRequest request)
        {
            if (ModelState.IsValid)
            {
                var test = new Test(
                    Guid.NewGuid(),
                    request.Theme,
                    request.Title,
                    request.Questions,
                    request.AnswerOptions,
                    request.CorrectAnswers,
                    request.Position,
                    request.CreatorId,
                    DateTime.Now,
                    DateTime.Now,
                    request.CourseId);

                await _testService.CreateTestAsync(test);

                return Ok();
            }
            return BadRequest("Invalid model object");
        }

        [HttpPut("UpdateTest/{id}")]
        [Authorize(Roles = "Admin,Creator")]
        public async Task<IActionResult> UpdateTest(Guid id, [FromBody] TestsRequest request)
        {
            if (ModelState.IsValid)
            {
                var test = new Test(
                    id,
                    request.Theme,
                    request.Title,
                    request.Questions,
                    request.AnswerOptions,
                    request.CorrectAnswers,
                    request.Position,
                    request.CreatorId,
                    DateTime.Parse(request.CreationDate),
                    DateTime.Now,
                    request.CourseId);

                var response = await _testService.UpdateTestAsync(test);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [HttpDelete("DeleteTest/{id}")]
        [Authorize(Roles = "Admin,Creator")]
        public async Task<IActionResult> DeleteTest(Guid id)
        {
            if (ModelState.IsValid)
            {
                var response = await _testService.DeleteTestAsync(id);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }
    }
}