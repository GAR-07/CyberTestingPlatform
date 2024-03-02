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

        public StorageService(
            ICoursesRepository coursesRepository,
            ILecturesRepository lecturesRepository)
        {
            _coursesRepository = coursesRepository;
            _lecturesRepository = lecturesRepository;
        }

        public async Task<(string[], string)> SaveFiles(IFormFileCollection files)
        {
            var error = ValidationFiles(files);
            string[] fileNames = new string[files.Count];
            string[] filePaths = new string[files.Count];
            string[][] contentType = new string[files.Count][];
            for (var i = 0; i < files.Count; i++)
            {
                var fileType = ContentDispositionHeaderValue.Parse(files[i].ContentDisposition).Name.Trim('"');
                var folderName = Path.Combine("Resources", fileType, SelectFolder());
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                fileNames[i] = ContentDispositionHeaderValue.Parse(files[i].ContentDisposition).FileName.Trim('"');
                var fullPath = RemoveFileNameCollision(Path.Combine(pathToSave, fileNames[i]));
                await using (FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write))
                {
                    await files[i].CopyToAsync(fileStream);
                }
                contentType[i] = files[i].ContentType.Split('/');
                filePaths[i] = Path.Combine(folderName, fullPath.Split(@"\").Last());
            }
            return (filePaths, error);
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
        public async Task<Guid> UpdateLecture(Guid id, string theme, string title, string content, DateTime lastUpdationDate, Guid courseId)
        {
            return await _lecturesRepository.Update(id, theme, title, content, lastUpdationDate, courseId);
        }
        public async Task<Guid> DeleteLecture(Guid userId)
        {
            return await _lecturesRepository.Delete(userId);
        }

        /// Далее идут дополнительные методы

        public DateTime ConvertToDateTime(string inputDate)
        {
            var dateSplit = inputDate.Split('-').Select(Int32.Parse).ToArray();
            return new DateTime(dateSplit[0], dateSplit[1], dateSplit[2]);
        }

        private static string ValidationFiles(IFormFileCollection files)
        {
            if (files.Count != 1)
            {
                return "Можно загружать только по одному файлу!";
            }
            foreach (var file in files)
            {
                if (file.Length == 0)
                {
                    return "Файл " + file.FileName + " имеет нулевой размер!";
                }
                if (file.Length > 100000000)
                {
                    return "Файл " + file.FileName + " слишком большой!";
                }
            }
            return "";
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
