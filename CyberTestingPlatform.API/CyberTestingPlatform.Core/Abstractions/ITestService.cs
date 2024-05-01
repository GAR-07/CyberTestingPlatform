using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.Application.Services
{
    public interface ITestService
    {
        Task<Guid> CreateTestAsync(Test test);
        Task<Guid> DeleteTestAsync(Guid id);
        Task<List<Test>> GetSelectTestsAsync(int sampleSize, int page);
        Task<Test> GetTestAsync(Guid id);
        Task<List<Test>> GetTestsByCourseIdAsync(Guid id);
        Task<Guid> UpdateTestAsync(Test test);
    }
}