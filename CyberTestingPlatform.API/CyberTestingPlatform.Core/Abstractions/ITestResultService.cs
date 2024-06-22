using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Application.Services
{
    public interface ITestResultService
    {
        Task<Guid> CreateTestResultAsync(TestResult testResult);
        Task<Guid> DeleteTestResultAsync(Guid id);
        Task<List<TestResult>> GetSelectionTestResultsByTest(string? searchText, int page, int pageSize, Guid testId);
        Task<List<TestResult>> GetSelectionTestResultsByUser(string? searchText, int page, int pageSize, Guid userId);
        Task<TestResult> GetTestResult(Guid id);
        Task<Guid> UpdateTestResultAsync(TestResult testResult);
    }
}