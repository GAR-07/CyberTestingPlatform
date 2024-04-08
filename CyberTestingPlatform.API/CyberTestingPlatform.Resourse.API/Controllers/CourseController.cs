using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Resourse.API.Models;
using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Resourse.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : Controller
    {
        private readonly IStorageService _storageService;

        public CourseController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        [HttpGet("GetCourse/{id}")]
        [Authorize]
        public async Task<IActionResult> GetCourse(Guid id)
        {
            if (ModelState.IsValid)
            {
                var course = await _storageService.GetCourse(id);

                if (course != null)
                {
                    var response = new CoursesResponse(
                        course.Id,
                        course.Name,
                        course.Description,
                        course.Price,
                        course.ImagePath,
                        course.CreatorID,
                        course.CreationDate,
                        course.LastUpdationDate);

                    return Ok(response);
                }
                return BadRequest("No objects found");
            }
            return BadRequest("Invalid model object");
        }

        [HttpGet]
        [Authorize]
        [Route("GetCourses")]
        public async Task<IActionResult> GetCourses([FromQuery] ItemsRequest request)
        {
            if (ModelState.IsValid)
            {
                var courses = await _storageService.GetSelectCourses(request.SampleSize, request.Page);

                if (courses != null)
                {
                    var response = courses.Select(x => new CoursesResponse(
                        x.Id,
                        x.Name,
                        x.Description,
                        x.Price,
                        x.ImagePath,
                        x.CreatorID,
                        x.CreationDate,
                        x.LastUpdationDate));

                    return Ok(response);
                }
                return BadRequest("No objects found");
            }
            return BadRequest("Invalid model object");
        }

        [Route("CreateCourse")]
        [Authorize(Roles = "Admin,Creator")]
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CoursesRequest request)
        {
            if (ModelState.IsValid)
            {
                var creationDate = _storageService.ConvertToDateTime(request.CreationDate);

                var (course, error) = Course.Create(
                    Guid.NewGuid(),
                    request.Name,
                    request.Description,
                    request.Price,
                    request.ImagePath,
                    request.CreatorId,
                    creationDate,
                    creationDate);

                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }
                
                var response = await _storageService.CreateCourse(course);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [HttpPut("UpdateCourse/{id}")]
        [Authorize(Roles = "Admin,Creator")]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] CoursesRequest request)
        {
            if (ModelState.IsValid)
            {
                var updationDate = _storageService.ConvertToDateTime(request.LastUpdationDate);

                var response = await _storageService.UpdateCourse(
                    id, 
                    request.Name, 
                    request.Description, 
                    request.Price, 
                    request.ImagePath, 
                    updationDate);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [HttpDelete("DeleteCourse/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            if (ModelState.IsValid)
            {
                var response = await _storageService.DeleteCourse(id);

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