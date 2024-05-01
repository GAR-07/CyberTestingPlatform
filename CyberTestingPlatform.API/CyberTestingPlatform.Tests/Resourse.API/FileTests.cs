using CyberTestingPlatform.Application.Services;
using CyberTestingPlatform.Resourse.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CyberTestingPlatform.DataAccess.Repositories;
using FakeItEasy;

namespace CyberTestingPlatform.Tests.Resourse.API
{
    public class FileTests
    {
        private readonly CoursesRepository _coursesRepository;
        private readonly LecturesRepository _lecturesRepository;
        private readonly TestsRepository _testsRepository;

        public FileTests() { }

        [Fact]
        public async Task UploadFiles_ValidFiles_ReturnsOk()
        {
            // Arrange
            if (Directory.Exists("Resources"))
            {
                Directory.Delete("Resources", true);
            }

            var fileData = new { FileName = "file1.jpg", ContentType = "image/jpeg", Size = 512 };

            var headers = new HeaderDictionary
            {
                { "Content-Disposition", new Microsoft.Extensions.Primitives.StringValues($"form-data; name=\"Image\"; filename=\"{fileData.FileName}\"") }
            };

            var formFile = new FormFile(Stream.Null, 0, fileData.Size, "Image", fileData.FileName)
            {
                Headers = headers,
                ContentType = fileData.ContentType
            };

            var formFileCollection = new FormFileCollection
            {
                formFile
            };

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Form = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>(), formFileCollection);

            var service = new StorageService();
            var controller = new StorageController(service);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Act
            var result = await controller.UploadFiles() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        }

        [Fact]
        public async Task SaveFiles_ValidFiles_ReturnsCorrectValues()
        {
            // Arrange
            if (Directory.Exists("Resources"))
            {
                Directory.Delete("Resources", true);
            }

            var files = new FormFileCollection();

            var fileData = new[]
            {
                new { FileName = "file1.jpg", ContentType = "image/jpeg", Size = 1},
                new { FileName = "file2.png", ContentType = "image/png", Size = 512},
                new { FileName = "file3.bmp", ContentType = "image/bmp", Size = 2048},
                new { FileName = "file4.gif", ContentType = "image/gif", Size = 104857600},
            };

            foreach (var data in fileData)
            {
                var headers = new HeaderDictionary
                {
                    { "Content-Disposition", new Microsoft.Extensions.Primitives.StringValues($"form-data; name=\"Image\"; filename=\"{data.FileName}\"") }
                };

                var formFile = new FormFile(Stream.Null, 0, data.Size, "Image", data.FileName)
                {
                    Headers = headers,
                    ContentType = data.ContentType
                };

                files.Add(formFile);
            }
            var service = new StorageService();

            // Act
            List<(string?, string?)> results = new();

            foreach (var file in files)
            {
                results.Add(await service.SaveFile(file));
            }

            // Assert
            Assert.Equal(results.Count, fileData.Length);

            for (int i = 0; i < results.Count; i++)
            {
                var (filePath, error) = results[i];
                var data = fileData[i];

                Assert.Empty(error);
                Assert.NotNull(filePath);

                Assert.Equal(data.FileName, Path.GetFileName(filePath));

                Assert.Equal(data.ContentType, GetContentType(Path.GetExtension(filePath)));
            }
        }

        [Fact]
        public async Task SaveFiles_InvalidFiles_ReturnsErrors()
        {
            // Arrange
            if (Directory.Exists("Resources"))
            {
                Directory.Delete("Resources", true);
            }

            var files = new FormFileCollection();

            var fileData = new[]
            {
                new { FileName = "file1.gif", ContentType = "image/webp", Size = 512},
                new { FileName = "file2.gif", ContentType = "image/svg+xml", Size = 512},
                new { FileName = "file3.txt", ContentType = "text/plain", Size = 512},
                new { FileName = "file4.jpg", ContentType = "image/jpeg", Size = -512},
                new { FileName = "file5.jpg", ContentType = "image/jpeg", Size = 0},
                new { FileName = "file4.gif", ContentType = "image/jpeg", Size = 104857601},
                new { FileName = "file6.jpg", ContentType = "image/jpeg", Size = 209715200},
            };

            foreach (var data in fileData)
            {
                var headers = new HeaderDictionary
                {
                    { "Content-Disposition", new Microsoft.Extensions.Primitives.StringValues($"form-data; name=\"Image\"; filename=\"{data.FileName}\"") }
                };

                var formFile = new FormFile(Stream.Null, 0, data.Size, "Image", data.FileName)
                {
                    Headers = headers,
                    ContentType = data.ContentType
                };

                files.Add(formFile);
            }
            var service = new StorageService();

            // Act
            List<(string?, string?)> results = new();

            foreach (var file in files)
            {
                results.Add(await service.SaveFile(file));
            }

            // Assert
            Assert.Equal(results.Count, fileData.Length);

            for (int i = 0; i < results.Count; i++)
            {
                var (filePath, error) = results[i];

                Assert.Empty(filePath);
                Assert.NotNull(error);
            }
        }

        static string GetContentType(string extension)
        {
            return extension.ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".bmp" => "image/bmp",
                ".gif" => "image/gif",
                ".mp4" => "video/mp4",
                _ => string.Empty,
            };
        }
    }
}