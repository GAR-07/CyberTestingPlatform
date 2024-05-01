using System.ComponentModel.DataAnnotations;
using CyberTestingPlatform.Core.Models;

namespace CyberTestingPlatform.DataAccess.Entites
{
    public class TestEntity
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MaxLength(Lecture.MAX_THEME_LENGTH)]
        public string Theme { get; set; }

        [Required]
        [MaxLength(Lecture.MAX_TITLE_LENGTH)]
        public string Title { get; set; }

        public string Questions { get; set; }

        public string AnswerOptions { get; set; }

        public string CorrectAnswers { get; set; }

        public int Position { get; set; }

        public Guid CreatorID { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime LastUpdationDate { get; set; }

        public Guid CourseId { get; set; }
    }
}