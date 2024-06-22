using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Application.Services
{
    public interface ILectureService
    {
        Task<Guid> CreateLectureAsync(Lecture lecture);
        Task<Guid> DeleteLectureAsync(Guid id);
        Task<Lecture> GetLectureAsync(Guid id);
        Task<List<Lecture>> GetLecturesByCourseIdAsync(Guid id);
        Task<List<Lecture>> GetSelectLecturesAsync(string? searchText, int page, int pageSize);
        Task<Guid> UpdateLectureAsync(Lecture lecture);
    }
}