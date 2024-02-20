using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public interface ICoursesRepository
    {
        Task<Guid> Create(Course lecture);
        Task<Guid> Delete(Guid id);
        Task<Course?> Get(Guid id);
        Task<List<Course>> GetAll();
        Task<List<Course>> GetSelection(int sampleSize, int page);
        Task<Guid> Update(Guid id, string theme, string title, string description, string content, int price, string imagePath, DateTime lastUpdationDate);
    }
}