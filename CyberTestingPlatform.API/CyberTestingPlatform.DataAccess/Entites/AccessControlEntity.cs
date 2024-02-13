using System.ComponentModel.DataAnnotations;

namespace CyberTestingPlatform.DataAccess.Entites
{
    public class AccessControlEntity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid CourseId { get; set; }

        public Guid LectureId { get; set; }

        public bool HasAccess { get; set; }
    }
}
