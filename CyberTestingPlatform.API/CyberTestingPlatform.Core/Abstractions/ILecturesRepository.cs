using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public interface ILecturesRepository
    {
        Task<Guid> Create(Lecture lecture);
        Task<Guid> Delete(Guid id);
        Task<Lecture?> Get(Guid id);
        Task<List<Lecture>> GetAll();
        Task<List<Lecture>> GetSelection(int sampleSize, int page);
        Task<Guid> Update(Guid id, string theme, string title, string content, DateTime lastUpdationDate, Guid courseId);
    }
}