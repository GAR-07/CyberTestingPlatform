using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public interface ITestResultsRepository
    {
        Task<Guid?> CreateAsync(TestResult testResult);
        Task<Guid?> DeleteAsync(Guid id);
        Task<TestResult?> GetAsync(Guid id);
        Task<List<TestResult>?> GetSelectionAsync(int sampleSize, int page, Guid userId);
        Task<Guid?> UpdateAsync(TestResult testResult);
    }
}