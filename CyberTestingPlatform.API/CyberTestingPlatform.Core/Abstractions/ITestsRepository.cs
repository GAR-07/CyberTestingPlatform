using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public interface ITestsRepository
    {
        Task<Guid?> CreateAsync(Test test);
        Task<Guid?> DeleteAsync(Guid id);
        Task<Test?> GetAsync(Guid id);
        Task<List<Test>?> GetByCourseIdAsync(Guid id);
        Task<List<Test>?> GetSelectionAsync(string? searchText, int page, int pageSize);
        Task<Guid?> UpdateAsync(Test test);
    }
}