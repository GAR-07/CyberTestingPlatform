using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public interface ICoursesRepository
    {
        Task<Guid> CreateAsync(Course course);
        Task<Guid> DeleteAsync(Guid id);
        Task<Course> GetAsync(Guid id);
        Task<List<Course>> GetSelectionAsync(int sampleSize, int page);
        Task<List<Course>> GetAllAsync();
        Task<Guid> UpdateAsync(Course course);
    }
}