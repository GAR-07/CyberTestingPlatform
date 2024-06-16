using CyberTestingPlatform.DataAccess.Repositories;
using CyberTestingPlatform.Core.Models;

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
        public async Task<List<TestResult>> GetSelectionTestResultsByUser(int sampleSize, int page, Guid userId)
        {
            if (sampleSize <= 0 || page < 0)
            {
                throw new Exception($"Заданы невозможные параметры для выборки");
            }
            return await _testResultsRepository.GetSelectionByUserAsync(sampleSize, page, userId) ?? throw new Exception($"Ничего не найдено");
        }
        public async Task<List<TestResult>> GetSelectionTestResultsByTest(int sampleSize, int page, Guid testId)
        {
            if (sampleSize <= 0 || page < 0)
            {
                throw new Exception($"Заданы невозможные параметры для выборки");
            }
            return await _testResultsRepository.GetSelectionByTestAsync(sampleSize, page, testId) ?? throw new Exception($"Ничего не найдено");
        }
        public async Task<TestResult> GetTestResult(Guid id)
        {
            return await _testResultsRepository.GetAsync(id) ?? throw new Exception($"Результат не найден");
        }
        public async Task<Guid> CreateTestResultAsync(TestResult testResult)
        {
            testResult.Results = await GetTestResults(testResult);

            return await _testResultsRepository.CreateAsync(testResult) ?? throw new Exception($"Ошибка сохранения результатов");
        }
        public async Task<Guid> UpdateTestResultAsync(TestResult testResult)
        {
            return await _testResultsRepository.UpdateAsync(testResult) ?? throw new Exception($"Результат {testResult.Id} не найден");
        }
        public async Task<Guid> DeleteTestResultAsync(Guid id)
        {
            return await _testResultsRepository.DeleteAsync(id) ?? throw new Exception($"Результат {id} не найден");
        }

        private async Task<string> GetTestResults(TestResult testResult)
        {
            var test = await _testService.GetTestAsync(testResult.TestId);

            string[] userAnswers = testResult.Answers.Split('\n');
            string[] correctAnswers = test.CorrectAnswers.Split('\n');
            var results = new List<bool>();

            if (userAnswers.Length != correctAnswers.Length)
            {
                throw new Exception($"Количество ответов не совпадает с вопросами");
            }

            for (int i = 0; i < correctAnswers.Length; i++)
            {
                results.Add(userAnswers[i].Equals(correctAnswers[i], StringComparison.OrdinalIgnoreCase));
            }

            return string.Join("\n", results.Select(x => x ? "Верно" : "Неверно"));
        }
    }
}