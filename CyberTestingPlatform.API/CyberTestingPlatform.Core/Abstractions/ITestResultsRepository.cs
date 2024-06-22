using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public interface ITestResultsRepository
    {
        Task<Guid?> CreateAsync(TestResult testResult);
        Task<Guid?> DeleteAsync(Guid id);
        Task<TestResult?> GetAsync(Guid id);
        Task<List<TestResult>?> GetSelectionByTestAsync(string? searchText, int page, int pageSize, Guid testId);
        Task<List<TestResult>?> GetSelectionByUserAsync(string? searchText, int page, int pageSize, Guid userId);
        Task<Guid?> UpdateAsync(TestResult testResult);
    }
}