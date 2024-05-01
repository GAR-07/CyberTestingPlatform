using System.ComponentModel.DataAnnotations;

namespace CyberTestingPlatform.DataAccess.Entites
{
    public class TestResultEntity
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid TestId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public string Answers { get; set; }

        public string Results { get; set; }

        public DateTime CreationDate { get; set; }
    }
}