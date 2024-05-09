namespace CyberTestingPlatform.Core.Models
{
    public class AccessControl
    {
        public static readonly DateTime MIN_DATE = new DateTime(2024, 1, 1);
        public static readonly DateTime MAX_DATE = DateTime.Now;

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ObjectId { get; set; }
        public bool HasAccess { get; set; }
        public string AccessType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public AccessControl(Guid id, Guid userId, Guid objectId, bool hasAccess, string accessType, DateTime startDate, DateTime? endDate)
        {
            Id = id;
            UserId = userId;
            ObjectId = objectId;
            HasAccess = hasAccess;
            AccessType = accessType;
            StartDate = startDate;
            EndDate = endDate;

            if (startDate < MIN_DATE || endDate > MAX_DATE)
            {
                throw new Exception($"Даты границ доступа не вписывается в допустимые временные рамки: ({MIN_DATE} - {MAX_DATE}");
            }
        }
    }
}