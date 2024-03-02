using System.ComponentModel.DataAnnotations;
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Entites
{
    public class CourseEntity
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(Course.MAX_NAME_LENGTH)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int Price { get; set; } = 0;

        public string ImagePath { get; set; }

        public Guid CreatorID { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdationDate { get; set; }
    }
}