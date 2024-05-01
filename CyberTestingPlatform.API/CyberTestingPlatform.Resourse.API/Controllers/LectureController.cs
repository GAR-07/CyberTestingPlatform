using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Resourse.API.Models;
using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Resourse.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LectureController : Controller
    {
        private readonly ILectureService _lectureService;

        public LectureController(ILectureService lectureService)
        {
            _lectureService = lectureService;
        }

        [HttpGet("GetLecturesByCourseId/{id}")]
        [Authorize]
        public async Task<IActionResult> GetLecturesByCourseId(Guid id)
        {
            if (ModelState.IsValid)
            {
                var lectures = await _lectureService.GetLecturesByCourseIdAsync(id);

                var response = lectures.Select(x => new LecturesResponse(
                    x.Id,
                    x.Theme,
                    x.Title,
                    x.Content,
                    x.Position,
                    x.CreatorID,
                    x.CreationDate,
                    x.LastUpdationDate,
                    x.CourseId));

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [HttpGet("GetLecture/{id}")]
        [Authorize]
        public async Task<IActionResult> GetLecture(Guid id)
        {
            if (ModelState.IsValid)
            {
                var lecture = await _lectureService.GetLectureAsync(id);

                var response = new LecturesResponse(
                    lecture.Id,
                    lecture.Theme,
                    lecture.Title,
                    lecture.Content,
                    lecture.Position,
                    lecture.CreatorID,
                    lecture.CreationDate,
                    lecture.LastUpdationDate,
                    lecture.CourseId);

                return Ok(response);
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
                var lectures = await _lectureService.GetSelectLecturesAsync(request.SampleSize, request.Page);

                var response = lectures.Select(x => new LecturesResponse(
                    x.Id,
                    x.Theme,
                    x.Title,
                    x.Content,
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
        [Route("CreateLecture")]
        public async Task<IActionResult> CreateLecture([FromBody] LecturesRequest request)
        {
            if (ModelState.IsValid)
            {
                var lecture = new Lecture(
                    Guid.NewGuid(),
                    request.Theme,
                    request.Title,
                    request.Content,
                    request.Position,
                    request.CreatorId,
                    DateTime.Now,
                    DateTime.Now,
                    request.CourseId);

                await _lectureService.CreateLectureAsync(lecture);

                return Ok();
            }
            return BadRequest("Invalid model object");
        }

        [HttpPut("UpdateLecture/{id}")]
        [Authorize(Roles = "Admin,Creator")]
        public async Task<IActionResult> UpdateLecture(Guid id, [FromBody] LecturesRequest request)
        {
            if (ModelState.IsValid)
            {
                var lecture = new Lecture(
                    id,
                    request.Theme,
                    request.Title,
                    request.Content,
                    request.Position,
                    request.CreatorId,
                    DateTime.Parse(request.CreationDate),
                    DateTime.Now,
                    request.CourseId);

                var response = await _lectureService.UpdateLectureAsync(lecture);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [HttpDelete("DeleteLecture/{id}")]
        [Authorize(Roles = "Admin,Creator")]
        public async Task<IActionResult> DeleteLecture(Guid id)
        {
            if (ModelState.IsValid)
            {
                var response = await _lectureService.DeleteLectureAsync(id);
                
                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }
    }
}