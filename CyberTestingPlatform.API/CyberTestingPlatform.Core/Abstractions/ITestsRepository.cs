using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public interface ITestsRepository
    {
        Task<Guid?> CreateAsync(Test test);
        Task<Guid?> DeleteAsync(Guid id);
        Task<Test?> GetAsync(Guid id);
        Task<List<Test>?> GetByCourseIdAsync(Guid id);
        Task<List<Test>?> GetSelectionAsync(int sampleSize, int page);
        Task<Guid?> UpdateAsync(Test test);
    }
}