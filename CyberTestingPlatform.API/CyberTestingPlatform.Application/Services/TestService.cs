using CyberTestingPlatform.DataAccess.Repositories;
using CyberTestingPlatform.Core.Models;
using CyberTestingPlatform.Core.Shared;

namespace CyberTestingPlatform.Application.Services
{
    public class TestService : ITestService
    {
        private readonly ITestsRepository _testsRepository;

        public TestService(ITestsRepository testsRepository)
        {
            _testsRepository = testsRepository;
        }

        public async Task<List<Test>> GetSelectTestsAsync(int sampleSize, int page)
        {
            if (sampleSize <= 0 || page < 0)
            {
                throw new CustomHttpException($"Заданы невозможные параметры для выборки", 422);
            }
            return await _testsRepository.GetSelectionAsync(sampleSize, page) ?? throw new CustomHttpException($"Ничего не найдено", 422);
        }
        public async Task<List<Test>> GetTestsByCourseIdAsync(Guid id)
        {
            return await _testsRepository.GetByCourseIdAsync(id) ?? throw new CustomHttpException($"Ничего не найдено", 422);
        }
        public async Task<Test> GetTestAsync(Guid id)
        {
            return await _testsRepository.GetAsync(id) ?? throw new CustomHttpException($"Тест {id} не найден", 422);
        }
        public async Task<Guid> CreateTestAsync(Test test)
        {
            return await _testsRepository.CreateAsync(test) ?? throw new CustomHttpException($"Ошибка создания лекции", 422);
        }
        public async Task<Guid> UpdateTestAsync(Test test)
        {
            return await _testsRepository.UpdateAsync(test) ?? throw new CustomHttpException($"Тест {test.Id} не найден", 422);
        }
        public async Task<Guid> DeleteTestAsync(Guid id)
        {
            return await _testsRepository.DeleteAsync(id) ?? throw new CustomHttpException($"Тест {id} не найден", 422);
        }
    }
}
