using CyberTestingPlatform.DataAccess.Repositories;
using CyberTestingPlatform.Core.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace CyberTestingPlatform.Application.Services
{
    public class StorageService : IStorageService
    {
        private readonly ICoursesRepository _coursesRepository;
        private readonly ILecturesRepository _lecturesRepository;
        private readonly ITestsRepository _testsRepository;

        public StorageService(
            ICoursesRepository coursesRepository,
            ILecturesRepository lecturesRepository,
            ITestsRepository testsRepository)
        {
            _coursesRepository = coursesRepository;
            _lecturesRepository = lecturesRepository;
            _testsRepository = testsRepository;
        }

        public async Task<(string, string)> SaveFile(IFormFile file)
        {
            string filePath = string.Empty;
            string error = ValidationFile(file);
            if (error == string.Empty)
            {
                var folderName = Path.Combine("Resources", GetContentType(file.ContentType), SelectFolder());
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = RemoveFileNameCollision(Path.Combine(pathToSave, fileName));
                await using (FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(fileStream);
                }
                filePath = Path.Combine(folderName, fullPath.Split(@"\").Last());
            }
            return (filePath, error);
        }

        // Далее идут методы для курсов

        public async Task<List<Course>> GetAllCourses()
        {
            return await _coursesRepository.GetAll();
        }
        public async Task<List<Course>?> GetSelectCourses(int sampleSize, int page)
        {
            if (sampleSize > 0 && page > 0)
            {
                return await _coursesRepository.GetSelection(sampleSize, page);
            }

            return null;
        }
        public async Task<Course?> GetCourse(Guid id)
        {
            return await _coursesRepository.Get(id);
        }
        public async Task<Guid> CreateCourse(Course course)
        {
            return await _coursesRepository.Create(course);
        }
        public async Task<Guid> UpdateCourse(Guid id, string name, string description, int price, string imagePath, DateTime lastUpdationDate)
        {
            return await _coursesRepository.Update(id, name, description, price, imagePath, lastUpdationDate);
        }
        public async Task<Guid> DeleteCourse(Guid userId)
        {
            return await _coursesRepository.Delete(userId);
        }

        // Далее идут методы для лекций

        public async Task<List<Lecture>> GetAllLectures()
        {
            return await _lecturesRepository.GetAll();
        }
        public async Task<List<Lecture>?> GetSelectLectures(int sampleSize, int page)
        {
            if (sampleSize > 0 && page > 0)
            {
                return await _lecturesRepository.GetSelection(sampleSize, page);
            }

            return null;
        }
        public async Task<Lecture?> GetLecture(Guid id)
        {
            return await _lecturesRepository.Get(id);
        }
        public async Task<Guid> CreateLecture(Lecture lecture)
        {
            return await _lecturesRepository.Create(lecture);
        }
        public async Task<Guid> UpdateLecture(Guid id, string theme, string title, string content, int position, DateTime lastUpdationDate, Guid courseId)
        {
            return await _lecturesRepository.Update(id, theme, title, content, position, lastUpdationDate, courseId);
        }
        public async Task<Guid> DeleteLecture(Guid userId)
        {
            return await _lecturesRepository.Delete(userId);
        }

        // Далее идут методы для тестов

        public async Task<List<Test>> GetAllTests()
        {
            return await _testsRepository.GetAll();
        }
        public async Task<List<Test>?> GetSelectTests(int sampleSize, int page)
        {
            if (sampleSize > 0 && page > 0)
            {
                return await _testsRepository.GetSelection(sampleSize, page);
            }

            return null;
        }
        public async Task<Test?> GetTest(Guid id)
        {
            return await _testsRepository.Get(id);
        }
        public async Task<Guid> CreateTest(Test test)
        {
            return await _testsRepository.Create(test);
        }
        public async Task<Guid> UpdateTest(Guid id, string theme, string title, string questions, string answerOptions, string answerCorrect, int position, DateTime lastUpdationDate, Guid courseId)
        {
            return await _testsRepository.Update(id, theme, title, questions, answerOptions, answerCorrect, position, lastUpdationDate, courseId);
        }
        public async Task<Guid> DeleteTest(Guid userId)
        {
            return await _testsRepository.Delete(userId);
        }

        // Далее идут дополнительные методы

        public DateTime ConvertToDateTime(string inputDate)
        {
            var dateSplit = inputDate.Split('-').Select(Int32.Parse).ToArray();
            return new DateTime(dateSplit[0], dateSplit[1], dateSplit[2]);
        }

        private static string ValidationFile(IFormFile file)
        {
            var allowedContentTypes = new List<string> { "image/jpeg", "image/png", "image/bmp", "image/gif", "video/mp4" };
            if (!allowedContentTypes.Contains(file.ContentType))
            {
                return "Файл " + file.FileName + " имеет неверный формат!";
            }
            if (file.Length <= 0)
            {
                return "Файл " + file.FileName + " имеет нулевой размер!";
            }
            if (file.Length > 104857600) // 100 MB
            {
                return "Файл " + file.FileName + " слишком большой!";
            }
            return string.Empty;
        }

        private static string RemoveFileNameCollision(string fullPath)
        {
            var collisionCount = 0;
            var fileName = fullPath.Split(@"\").Last();
            var pathToSave = fullPath[..fullPath.LastIndexOf(@"\")];
            while (System.IO.File.Exists(fullPath))
            {
                collisionCount++;
                var newFileName = $"{fileName[..fileName.LastIndexOf('.')]}({collisionCount}).{fileName.Split('.').Last()}";
                fullPath = Path.Combine(pathToSave, newFileName);
            }
            return fullPath;
        }

        private static string GetContentType(string contentType)
        {
            return contentType switch
            {
                "image/jpeg" => "Images",
                "image/png" => "Images",
                "image/bmp" => "Images",
                "image/gif" => "Images",
                "video/mp4" => "Videos",
                _ => "Forbidden",
            };
        }

        private static string SelectFolder()
        {
            Random rnd = new();
            return hexValues[rnd.Next(0, 15)];
        }

        private static readonly string[] hexValues = new string[]
        { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
    }
}
