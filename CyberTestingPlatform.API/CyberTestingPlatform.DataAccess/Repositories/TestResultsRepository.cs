using CyberTestingPlatform.Core.Models;
using CyberTestingPlatform.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public class TestResultsRepository : ITestResultsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TestResultsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<TestResult>?> GetSelectionAsync(int sampleSize, int page, Guid userId)
        {
            var totalCount = await _dbContext.TestResults.AsNoTracking().Where(x => x.UserId == userId).CountAsync();
            var startIndex = Math.Max(0, totalCount - sampleSize * page);
            var countToTake = Math.Min(sampleSize, totalCount - startIndex);

            var testResultEntities = await _dbContext.TestResults
                .Where(x => x.UserId == userId)
                .Skip(startIndex)
                .Take(countToTake)
                .AsNoTracking()
                .ToListAsync();

            if (testResultEntities == null)
            {
                return null;
            }

            var results = testResultEntities
               .Select(x => new TestResult(x.Id, x.TestId, x.UserId, x.Answers, x.Results, x.CreationDate))
               .ToList();

            return results;
        }

        public async Task<TestResult?> GetAsync(Guid id)
        {
            var testResultEntities = await _dbContext.TestResults.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

            if (testResultEntities == null)
            {
                return null;
            }

            return new TestResult(
                testResultEntities.Id,
                testResultEntities.TestId,
                testResultEntities.UserId,
                testResultEntities.Answers,
                testResultEntities.Results,
                testResultEntities.CreationDate);
        }

        public async Task<Guid?> CreateAsync(TestResult testResult)
        {
            var testResultEntities = new TestResultEntity
            {
                Id = testResult.Id,
                TestId = testResult.TestId,
                UserId = testResult.UserId,
                Answers = testResult.Answers,
                Results = testResult.Results,
                CreationDate = testResult.CreationDate
            };

            await _dbContext.TestResults.AddAsync(testResultEntities);
            await _dbContext.SaveChangesAsync();

            return testResultEntities.Id;
        }

        public async Task<Guid?> UpdateAsync(TestResult testResult)
        {
            var testResultEntities = await _dbContext.TestResults
                .Where(p => p.Id == testResult.Id)
                .FirstOrDefaultAsync();

            if (testResultEntities == null)
            {
                return null;
            }

            testResultEntities.TestId = testResult.TestId;
            testResultEntities.UserId = testResult.UserId;
            testResultEntities.Answers = testResult.Answers;
            testResultEntities.Results = testResult.Results;
            testResultEntities.CreationDate = testResult.CreationDate;

            await _dbContext.SaveChangesAsync();

            return testResultEntities.Id;
        }

        public async Task<Guid?> DeleteAsync(Guid id)
        {
            var testResultEntities = await _dbContext.TestResults
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (testResultEntities == null)
            {
                return null;
            }

            _dbContext.Remove(testResultEntities);
            await _dbContext.SaveChangesAsync();

            return id;
        }
    }
}
