using CyberTestingPlatform.DataAccess.Repositories;
using CyberTestingPlatform.Core.Models;
using CyberTestingPlatform.Core.Shared;
using static System.Net.Mime.MediaTypeNames;

namespace CyberTestingPlatform.Application.Services
{
    public class TestResultService : ITestResultService
    {
        private readonly ITestResultsRepository _testResultsRepository;
        private readonly ITestService _testService;

        public TestResultService(ITestResultsRepository testResultsRepository, ITestService testService)
        {
            _testResultsRepository = testResultsRepository;
            _testService = testService;
        }

        public async Task<List<TestResult>> GetSelectionTestResultsId(int sampleSize, int page, Guid id)
        {
            if (sampleSize <= 0 || page < 0)
            {
                throw new CustomHttpException($"Заданы невозможные параметры для выборки", 422);
            }
            return await _testResultsRepository.GetSelectionAsync(sampleSize, page, id) ?? throw new CustomHttpException($"Ничего не найдено", 422);
        }
        public async Task<TestResult> GetTestResult(Guid id)
        {
            return await _testResultsRepository.GetAsync(id) ?? throw new CustomHttpException($"Результат не найден", 422);
        }
        public async Task<Guid> CreateTestResultAsync(TestResult testResult)
        {
            var test = await _testService.GetTestAsync(testResult.TestId);

            string[] userAnswers = testResult.Answers.Split('\n');
            string[] correctAnswers = test.CorrectAnswers.Split('\n');
            var results = new List<bool>();

            if (userAnswers.Length != correctAnswers.Length)
            {
                throw new CustomHttpException($"Количество ответов не совпадает с вопросами", 422);
            }

            for (int i = 0; i < correctAnswers.Length; i++)
            {
                results.Add(userAnswers[i].Equals(correctAnswers[i], StringComparison.OrdinalIgnoreCase));
            }

            testResult.Results =  string.Join("\n", results.Select(x => x ? "Верно" : "Неверно"));

            return await _testResultsRepository.CreateAsync(testResult) ?? throw new CustomHttpException($"Ошибка сохранения результатов", 422);
        }
        public async Task<Guid> UpdateTestResultAsync(TestResult testResult)
        {
            return await _testResultsRepository.UpdateAsync(testResult) ?? throw new CustomHttpException($"Результат {testResult.Id} не найден", 422);
        }
        public async Task<Guid> DeleteTestResultAsync(Guid id)
        {
            return await _testResultsRepository.DeleteAsync(id) ?? throw new CustomHttpException($"Результат {id} не найден", 422);
        }
    }
}