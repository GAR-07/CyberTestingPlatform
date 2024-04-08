using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public interface ITestsRepository
    {
        Task<Guid> Create(Test test);
        Task<Guid> Delete(Guid id);
        Task<Test?> Get(Guid id);
        Task<List<Test>> GetAll();
        Task<List<Test>> GetSelection(int sampleSize, int page);
        Task<Guid> Update(Guid id, string theme, string title, string questions, string answerOptions, string answerCorrect, int position, DateTime lastUpdationDate, Guid courseId);
    }
}