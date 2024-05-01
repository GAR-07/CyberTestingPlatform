namespace CyberTestingPlatform.Core.Models
{
    public class TestResult(Guid id, Guid testId, Guid userId, string answers, string results, DateTime creationDate)
    {
        public Guid Id { get; set; } = id;
        public Guid TestId { get; set; } = testId;
        public Guid UserId { get; set; } = userId;
        public string Answers { get; set; } = answers;
        public string Results { get; set; } = results;
        public DateTime CreationDate { get; set; } = creationDate;
    }
}