using CyberTestingPlatform.Core.Models;
using CyberTestingPlatform.DataAccess.Entites;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CyberTestingPlatform.DataAccess.Repositories
{
    public class TestsRepository : ITestsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TestsRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Test>> GetAll()
        {
            var testEntities = await _dbContext.Tests
                .AsNoTracking()
                .ToListAsync();

            var tests = testEntities
                .Select(x => Test.Create(x.Id, x.Theme, x.Title, x.Questions, x.AnswerOptions, x.AnswerCorrect, x.Position, x.CreatorID, x.CreationDate, x.LastUpdationDate, x.CourseId).test)
                .ToList();

            return tests;
        }

        public async Task<List<Test>> GetSelection(int sampleSize, int page)
        {
            var totalCount = await _dbContext.Tests.CountAsync();
            var startIndex = Math.Max(0, totalCount - sampleSize * page);
            var countToTake = Math.Min(sampleSize, totalCount - startIndex);

            var testEntities = await _dbContext.Tests
                .Skip(startIndex)
                .Take(countToTake)
                .AsNoTracking()
                .ToListAsync();

            var tests = testEntities
               .Select(x => Test.Create(x.Id, x.Theme, x.Title, x.Questions, x.AnswerOptions, x.AnswerCorrect, x.Position, x.CreatorID, x.CreationDate, x.LastUpdationDate, x.CourseId).test)
               .ToList();

            return tests;
        }

        public async Task<Test?> Get(Guid id)
        {
            var testEntity = await _dbContext.Tests.FirstOrDefaultAsync(p => p.Id == id);
            return testEntity != null ? Test.Create(
                testEntity.Id,
                testEntity.Theme,
                testEntity.Title,
                testEntity.Questions,
                testEntity.AnswerOptions,
                testEntity.AnswerCorrect,
                testEntity.Position,
                testEntity.CreatorID,
                testEntity.CreationDate,
                testEntity.LastUpdationDate,
                testEntity.CourseId).test
                : null;
        }

        public async Task<Guid> Create(Test test)
        {
            var testEntity = new TestEntity
            {
                Id = test.Id,
                Theme = test.Theme,
                Title = test.Title,
                Questions = test.Questions,
                AnswerOptions = test.AnswerOptions,
                AnswerCorrect = test.AnswerCorrect,
                Position = test.Position,
                CreatorID = test.CreatorID,
                CreationDate = test.CreationDate,
                LastUpdationDate = test.LastUpdationDate,
                CourseId = test.CourseId
            };

            await _dbContext.Tests.AddAsync(testEntity);
            await _dbContext.SaveChangesAsync();

            return testEntity.Id;
        }

        public async Task<Guid> Update(Guid id, string theme, string title, string questions, string answerOptions, string answerCorrect, int position, DateTime lastUpdationDate, Guid courseId)
        {
            var testEntity = await _dbContext.Tests
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (testEntity != null)
            {
                testEntity.Theme = theme;
                testEntity.Title = title;
                testEntity.Questions = questions;
                testEntity.AnswerOptions = answerOptions;
                testEntity.AnswerCorrect = answerCorrect;
                testEntity.Position = position;
                testEntity.LastUpdationDate = lastUpdationDate;
                testEntity.CourseId = courseId;

                await _dbContext.SaveChangesAsync();

                return id;
            }

            return Guid.Empty;
        }

        public async Task<Guid> Delete(Guid id)
        {
            var testEntity = await _dbContext.Tests
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();

            if (testEntity != null)
            {
                _dbContext.Remove(testEntity);
                await _dbContext.SaveChangesAsync();

                return id;
            }

            return Guid.Empty;
        }
    }
}
