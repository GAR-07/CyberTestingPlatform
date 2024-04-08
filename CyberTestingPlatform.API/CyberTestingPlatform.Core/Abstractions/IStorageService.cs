using CyberTestingPlatform.Core.Models;
using Microsoft.AspNetCore.Http;

namespace CyberTestingPlatform.Application.Services
{
    public interface IStorageService
    {
        DateTime ConvertToDateTime(string inputDate);
        Task<Guid> CreateCourse(Course course);
        Task<Guid> CreateLecture(Lecture lecture);
        Task<Guid> CreateTest(Test test);
        Task<Guid> DeleteCourse(Guid userId);
        Task<Guid> DeleteLecture(Guid userId);
        Task<Guid> DeleteTest(Guid userId);
        Task<List<Course>> GetAllCourses();
        Task<List<Lecture>> GetAllLectures();
        Task<List<Test>> GetAllTests();
        Task<Course?> GetCourse(Guid id);
        Task<Lecture?> GetLecture(Guid id);
        Task<List<Course>?> GetSelectCourses(int sampleSize, int page);
        Task<List<Lecture>?> GetSelectLectures(int sampleSize, int page);
        Task<List<Test>?> GetSelectTests(int sampleSize, int page);
        Task<Test?> GetTest(Guid id);
        Task<(string, string)> SaveFile(IFormFile file);
        Task<Guid> UpdateCourse(Guid id, string name, string description, int price, string imagePath, DateTime lastUpdationDate);
        Task<Guid> UpdateLecture(Guid id, string theme, string title, string content, int position, DateTime lastUpdationDate, Guid courseId);
        Task<Guid> UpdateTest(Guid id, string theme, string title, string questions, string answerOptions, string answerCorrect, int position, DateTime lastUpdationDate, Guid courseId);
    }
}