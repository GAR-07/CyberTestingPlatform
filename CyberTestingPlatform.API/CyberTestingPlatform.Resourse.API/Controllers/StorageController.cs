using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Resourse.API.Models;
using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Resourse.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StorageController : Controller
    {
        private readonly IStorageService _storageService;

        public StorageController(IStorageService storageService)
        {
            _storageService = storageService;
        }

        // Далее идут методы для курсов

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
                        course.Theme,
                        course.Title,
                        course.Description,
                        course.Content,
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
                        x.Theme,
                        x.Title,
                        x.Description,
                        x.Content,
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
                    request.Theme,
                    request.Title,
                    request.Description,
                    request.Content,
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
                    request.Theme, 
                    request.Title, 
                    request.Description, 
                    request.Content, 
                    request.Price, 
                    request.ImagePath, 
                    updationDate);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        // Далее идут методы для лекций

        [HttpGet("GetLecture/{id}")]
        [Authorize]
        public async Task<IActionResult> GetLecture(Guid id)
        {
            if (ModelState.IsValid)
            {
                var lecture = await _storageService.GetLecture(id);

                if (lecture != null)
                {
                    var response = new LecturesResponse(
                        lecture.Id,
                        lecture.Theme,
                        lecture.Title,
                        lecture.Content,
                        lecture.CreatorID,
                        lecture.CreationDate,
                        lecture.LastUpdationDate,
                        lecture.CourseId);

                    return Ok(response);
                }
                return BadRequest("No objects found");
            }
            return BadRequest("Invalid model object");
        }

        [HttpGet]
        [Authorize]
        [Route("GetLectures")]
        public async Task<IActionResult> GetLectures([FromQuery] ItemsRequest request)
        {
            if (ModelState.IsValid)
            {
                var lectures = await _storageService.GetSelectLectures(request.SampleSize, request.Page);

                if (lectures != null)
                {
                    var response = lectures.Select(x => new LecturesResponse(
                        x.Id,
                        x.Theme,
                        x.Title,
                        x.Content,
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

        [Route("CreateLecture")]
        [Authorize(Roles = "Admin,Creator")]
        [HttpPost]
        public async Task<IActionResult> CreateLecture([FromBody] LecturesRequest request)
        {
            if (ModelState.IsValid)
            {
                var creationDate = _storageService.ConvertToDateTime(request.CreationDate);

                var (lecture, error) = Lecture.Create(
                    Guid.NewGuid(),
                    request.Theme,
                    request.Title,
                    request.Content,
                    request.CreatorId,
                    creationDate,
                    creationDate,
                    request.CourseId);

                if (!string.IsNullOrEmpty(error))
                {
                    return BadRequest(error);
                }

                var response = await _storageService.CreateLecture(lecture);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [HttpPut("UpdateLecture/{id}")]
        [Authorize(Roles = "Admin,Creator")]
        public async Task<IActionResult> UpdateLecture(Guid id, [FromBody] LecturesRequest request)
        {
            if (ModelState.IsValid)
            {
                var updationDate = _storageService.ConvertToDateTime(request.LastUpdationDate);

                var response = await _storageService.UpdateLecture(
                    id, 
                    request.Theme, 
                    request.Title, 
                    request.Content, 
                    updationDate, 
                    request.CourseId);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }
    }
}