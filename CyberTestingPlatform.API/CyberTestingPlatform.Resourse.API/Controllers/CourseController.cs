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
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet("GetCourse/{id}")]
        public async Task<IActionResult> GetCourse(Guid id)
        {
            if (ModelState.IsValid)
            {
                var course = await _courseService.GetCourseAsync(id);

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
            return BadRequest("Invalid model object");
        }

        [Authorize]
        [HttpGet("GetCourses")]
        public async Task<IActionResult> GetCourses([FromQuery] SearchRequest request)
        {
            if (ModelState.IsValid)
            {
                var courses = await _courseService.GetSelectCoursesAsync(request.SearchText, request.Page, request.PageSize);

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
            return BadRequest("Invalid model object");
        }

        [Authorize]
        [HttpGet("GetAllCourses")]
        public async Task<IActionResult> GetAllCourses()
        {
            if (ModelState.IsValid)
            {
                var courses = await _courseService.GetAllCoursesAsync();

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
            return BadRequest("Invalid model object");
        }

        [Authorize(Roles = "Admin,Creator")]
        [HttpPost("CreateCourse")]
        public async Task<IActionResult> CreateCourse([FromBody] CoursesRequest request)
        {
            if (ModelState.IsValid)
            {
                var course = new Course(
                    Guid.NewGuid(),
                    request.Name,
                    request.Description,
                    request.Price,
                    request.ImagePath,
                    request.CreatorId,
                    DateTime.Now,
                    DateTime.Now);
                
                await _courseService.CreateCourseAsync(course);

                return Ok();
            }
            return BadRequest("Invalid model object");
        }

        [Authorize(Roles = "Admin,Creator")]
        [HttpPut("UpdateCourse/{id}")]
        public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] CoursesRequest request)
        {
            if (ModelState.IsValid)
            {
                var course = new Course(
                    id,
                    request.Name,
                    request.Description,
                    request.Price,
                    request.ImagePath,
                    request.CreatorId,
                    DateTime.Parse(request.CreationDate),
                    DateTime.Now);

                var response = await _courseService.UpdateCourseAsync(course);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [Authorize(Roles = "Admin,Creator")]
        [HttpDelete("DeleteCourse/{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            if (ModelState.IsValid)
            {
                var response = await _courseService.DeleteCourseAsync(id);
                
                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }
    }
}