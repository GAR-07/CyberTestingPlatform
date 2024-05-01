using CyberTestingPlatform.DataAccess.Repositories;
using CyberTestingPlatform.Core.Models;

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
                throw new Exception($"Заданы невозможные параметры для выборки");
            }
            return await _testsRepository.GetSelectionAsync(sampleSize, page);
        }
        public async Task<List<Test>> GetTestsByCourseIdAsync(Guid id)
        {
            return await _testsRepository.GetByCourseIdAsync(id);
        }
        public async Task<Test> GetTestAsync(Guid id)
        {
            return await _testsRepository.GetAsync(id);
        }
        public async Task<Guid> CreateTestAsync(Test test)
        {
            return await _testsRepository.CreateAsync(test);
        }
        public async Task<Guid> UpdateTestAsync(Test test)
        {
            return await _testsRepository.UpdateAsync(test);
        }
        public async Task<Guid> DeleteTestAsync(Guid id)
        {
            return await _testsRepository.DeleteAsync(id);
        }

        // Далее идут методы для результатов тестов

        //public async Task<List<Test>?> GetResultByTestId(Guid id)
        //{
        //    return await _testsRepository.GetResultByTestId(id);
        //}
        //public async Task<Test?> GetTest(Guid id)
        //{
        //    return await _testsRepository.Get(id);
        //}
    }
}
