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

        [Authorize]
        [HttpGet("GetLecturesByCourseId/{id}")]
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

        [Authorize]
        [HttpGet("GetLecture/{id}")]
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

        [Authorize]
        [HttpGet("GetLectures")]
        public async Task<IActionResult> GetLectures([FromQuery] SearchRequest request)
        {
            if (ModelState.IsValid)
            {
                var lectures = await _lectureService.GetSelectLecturesAsync(request.SearchText, request.Page, request.PageSize);

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

        [Authorize(Roles = "Admin,Creator")]
        [HttpPost("CreateLecture")]
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

        [Authorize(Roles = "Admin,Creator")]
        [HttpPut("UpdateLecture/{id}")]
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

        [Authorize(Roles = "Admin,Creator")]
        [HttpDelete("DeleteLecture/{id}")]
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