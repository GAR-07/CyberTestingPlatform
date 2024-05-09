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

        [Authorize]
        [HttpGet("GetTestsByCourseId/{id}")]
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

        [Authorize]
        [HttpGet("GetTest/{id}")]
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

        [Authorize]
        [HttpGet("GetTests")]
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

        [Authorize(Roles = "Admin,Creator")]
        [HttpPost("CreateTest")]
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

        [Authorize(Roles = "Admin,Creator")]
        [HttpPut("UpdateTest/{id}")]
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

        [Authorize(Roles = "Admin,Creator")]
        [HttpDelete("DeleteTest/{id}")]
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