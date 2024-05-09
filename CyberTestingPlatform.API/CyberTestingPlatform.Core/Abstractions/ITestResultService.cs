using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Application.Services
{
    public interface ITestResultService
    {
        Task<Guid> CreateTestResultAsync(TestResult testResult);
        Task<Guid> DeleteTestResultAsync(Guid id);
        Task<List<TestResult>> GetSelectionTestResultsByTest(int sampleSize, int page, Guid testId);
        Task<List<TestResult>> GetSelectionTestResultsByUser(int sampleSize, int page, Guid userId);
        Task<TestResult> GetTestResult(Guid id);
        Task<Guid> UpdateTestResultAsync(TestResult testResult);
    }
}