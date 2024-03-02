namespace CyberTestingPlatform.Core.Models
{
    public class Course
    {
        public const int MAX_NAME_LENGTH = 255;

        private Course(Guid id, string name, string description, int price, string imagePath, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            ImagePath = imagePath;
            CreatorID = creatorID;
            CreationDate = creationDate;
            LastUpdationDate = lastUpdationDate;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }
        public int Price { get; }
        public string ImagePath { get; }
        public Guid CreatorID { get; }
        public DateTime CreationDate { get; }
        public DateTime LastUpdationDate { get; }

        public static (Course course, string error) Create(Guid id, string name, string description, int price, string imagePath, Guid creatorID, DateTime creationDate, DateTime lastUpdationDate)
        {
            var error = string.Empty;

            if (string.IsNullOrEmpty(name) || name.Length > MAX_NAME_LENGTH)
            {
                error = $"Тема не может быть пустой или превышать {MAX_NAME_LENGTH} символов";
            }
            if (price < 0)
            {
                error = $"Цена не может иметь отрицательное значение";
            }

            var course = new Course(id, name, description, price, imagePath, creatorID, creationDate, lastUpdationDate);

            return (course, error);
        }
    }
}
