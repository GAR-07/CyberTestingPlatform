using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public interface ICoursesRepository
    {
        Task<Guid?> CreateAsync(Course course);
        Task<Guid?> DeleteAsync(Guid id);
        Task<Course?> GetAsync(Guid id);
        Task<List<Course>?> GetSelectionAsync(string? searchText, int page, int pageSize);
        Task<List<Course>?> GetAllAsync();
        Task<Guid?> UpdateAsync(Course course);
    }
}