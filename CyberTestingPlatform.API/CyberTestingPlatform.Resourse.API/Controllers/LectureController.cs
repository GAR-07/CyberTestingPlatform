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
        private readonly IStorageService _storageService;

        public LectureController(IStorageService storageService)
        {
            _storageService = storageService;
        }

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
                        lecture.Position,
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
        [Route("CreateLecture")]
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
                    request.Position,
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
                    request.Position,
                    updationDate, 
                    request.CourseId);

                return Ok(response);
            }
            return BadRequest("Invalid model object");
        }

        [HttpDelete("DeleteLecture/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteLecture(Guid id)
        {
            if (ModelState.IsValid)
            {
                var response = await _storageService.DeleteLecture(id);

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