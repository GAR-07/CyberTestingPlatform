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
        private readonly IStorageService _storageService;

        public TestController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpGet("Gettest/{id}")]
        [Authorize]
        public async Task<IActionResult> Gettest(Guid id)
        {
            if (ModelState.IsValid)
            {
                var test = await _storageService.GetTest(id);

                if (test != null)
                {
                    var response = new TestsResponse(
                        test.Id,
                        test.Theme,
                        test.Title,
                        test.Questions,
                        test.AnswerOptions,
                        test.AnswerCorrect,
                        test.Position,
                        test.CreatorID,
                        test.CreationDate,
                        test.LastUpdationDate,
                        test.CourseId);

                    return Ok(response);
                }
                return BadRequest("No objects found");
            }
            return BadRequest("Invalid model object");
        }

        [HttpGet]
        [Authorize]
        [Route("Gettests")]
        public async Task<IActionResult> Gettests([FromQuery] ItemsRequest request)
        {
            if (ModelState.IsValid)
            {
                var tests = await _storageService.GetSelectTests(request.SampleSize, request.Page);

                if (tests != null)
                {
                    var response = tests.Select(x => new TestsResponse(
                        x.Id,
                        x.Theme,
                        x.Title,
                        x.Questions,
                        x.AnswerOptions,
                        x.AnswerCorrect,
                        x.Position,
                        x.CreatorID,
                        x.CreationDate,
                        x.LastUpdationDate,
                        x.CourseId));

                    return Ok(response);
                }
                return BadRequest("No objects found");
            }
            return BadRequest("Invalid model object");
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Creator")]
        [Route("Createtest")]
        public async Task<IActionResult> Createtest([FromBody] TestsRequest request)
        {
            if (ModelState.IsValid)
            {
                var creationDate = _storageService.ConvertToDateTime(request.CreationDate);

                var (test, error) = Test.Create(
                    Guid.NewGuid(),
                    request.Theme,
                    request.Title,
                    request.Questions,
                    request.AnswerOptions,
                    request.AnswerCorrect,
                    request.Position,
                    request.CreatorId,
                    creationDate,
                    creationDate,
                    request.CourseId);

                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }

                var response = await _storageService.CreateTest(test);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [HttpPut("Updatetest/{id}")]
        [Authorize(Roles = "Admin,Creator")]
        public async Task<IActionResult> Updatetest(Guid id, [FromBody] TestsRequest request)
        {
            if (ModelState.IsValid)
            {
                var updationDate = _storageService.ConvertToDateTime(request.LastUpdationDate);

                var response = await _storageService.UpdateTest(
                    id, 
                    request.Theme, 
                    request.Title,
                    request.Questions,
                    request.AnswerOptions,
                    request.AnswerCorrect,
                    request.Position, 
                    updationDate, 
                    request.CourseId);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [HttpDelete("Deletetest/{id}")]
        [Authorize]
        public async Task<IActionResult> Deletetest(Guid id)
        {
            if (ModelState.IsValid)
            {
                var response = await _storageService.DeleteTest(id);

                if (response != Guid.Empty)
                {
                    return Ok(response);
                }
                return BadRequest("No objects found");
            }
            return BadRequest("Invalid model object");
        }
    }
}