using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.Resourse.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.Resourse.API.Models;
using CyberTestingPlatform.Core.Models;
using CyberTestingPlatform.DataAccess.Repositories;
using FakeItEasy;

namespace CyberTestingPlatform.Tests.Resourse.API
{
    public class CourseTests
    {
        private readonly IStorageService _storageService;
        private readonly CoursesRepository _coursesRepository;
        private readonly LecturesRepository _lecturesRepository;
        private readonly TestsRepository _testsRepository;

        public CourseTests() { }

        [Fact]
        public async Task CreateCourse_ValidRequest_ReturnsOk()
        {
            // Arrange
            var request = new CoursesRequest(
                Name: "Test Course",
                Description: "Test Description",
                Price: 100,
                ImagePath: "Resources\\Images\\0\\test.jpg",
                CreatorId: Guid.NewGuid(),
                CreationDate: "2020-2-20",
                LastUpdationDate: "");

            var expectedCourseId = Guid.NewGuid();

            var coursesRepository = A.Fake<ICoursesRepository>();
            A.CallTo(() => coursesRepository.Create(A<Course>._))
                .Returns(Task.FromResult(expectedCourseId));

            var service = new StorageService(coursesRepository, _lecturesRepository, _testsRepository);
            var controller = new CourseController(service);

            // Act
            var result = await controller.CreateCourse(request) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);

            var response = result.Value as dynamic;
            Assert.Equal(expectedCourseId, response);
        }

        [Fact]
        public async Task CreateCourse_InvalidRequest_ReturnsBadRequest()
        {
            // Arrange
            var request = new CoursesRequest(
                Name: "Test Course",
                Description: "Test Description",
                Price: 100,
                ImagePath: "Resources\\Images\\0\\test.jpg",
                CreatorId: Guid.NewGuid(),
                CreationDate: "2020-2-20",
                LastUpdationDate: "");

            var controller = new CourseController(_storageService);
            controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await controller.CreateCourse(request) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }
    }
}