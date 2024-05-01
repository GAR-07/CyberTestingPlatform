namespace CyberTestingPlatform.Core.Models
{
    public class Course(Guid id, string name, string description, int price, string imagePath, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate)
    {
        public const int MAX_NAME_LENGTH = 255;

        public Guid Id { get; set; } = id;
        public string Name { get; set; } = name;
        public string Description { get; set; } = description;
        public int Price { get; set; } = price;
        public string ImagePath { get; set; } = imagePath;
        public Guid CreatorID { get; set; } = creatorID;
        public DateTime CreationDate { get; set; } = creationDate;
        public DateTime LastUpdationDate { get; set; } = lastUpdationDate;
    }
}
