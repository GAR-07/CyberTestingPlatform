using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Application.Services
{
    public interface IStorageService
    {
        DateTime ConvertToDateTime(string inputDate);
        Task<Guid> CreateCourse(Course course);
        Task<Guid> CreateLecture(Lecture lecture);
        Task<Guid> DeleteCourse(Guid userId);
        Task<Guid> DeleteLecture(Guid userId);
        Task<List<Course>> GetAllCourses();
        Task<List<Lecture>> GetAllLectures();
        Task<Course?> GetCourse(Guid id);
        Task<Lecture?> GetLecture(Guid id);
        Task<List<Course>?> GetSelectCourses(int sampleSize, int page);
        Task<List<Lecture>?> GetSelectLectures(int sampleSize, int page);
        Task<Guid> UpdateCourse(Guid id, string theme, string title, string description, string content, int price, string imagePath, DateTime lastUpdationDate);
        Task<Guid> UpdateLecture(Guid id, string theme, string title, string content, DateTime lastUpdationDate, Guid courseId);
    }
}