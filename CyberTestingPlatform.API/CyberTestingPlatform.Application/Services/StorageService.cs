using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;

namespace CyberTestingPlatform.Application.Services
{
    public class StorageService : IStorageService
    {
        public StorageService() { }

        public async Task<string> SaveFile(IFormFile file)
        {
            var folderName = Path.Combine("Resources", GetContentType(file.ContentType), SelectFolder());
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fullPath = RemoveFileNameCollision(Path.Combine(pathToSave, fileName));

            await using (FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write))
            {
                await file.CopyToAsync(fileStream);
            }

            var filePath = Path.Combine(folderName, fullPath.Split(@"\").Last());

            return filePath;
        }

        public void ValidationImageFile(IFormFile file)
        {
            var allowedContentTypes = new List<string> { "image/jpeg", "image/png", "image/bmp", "image/gif", "video/mp4" };

            if (!allowedContentTypes.Contains(file.ContentType))
                throw new Exception($"Файл " + file.FileName + " имеет неверный формат!");

            if (file.Length <= 0)
                throw new Exception($"Файл " + file.FileName + " имеет нулевой размер!");

            if (file.Length > 104857600) // 100 MB
                throw new Exception($"Файл " + file.FileName + " слишком большой!");
        }

        private static string RemoveFileNameCollision(string fullPath)
        {
            var collisionCount = 0;
            var fileName = fullPath.Split(@"\").Last();
            var pathToSave = fullPath[..fullPath.LastIndexOf(@"\")];
            while (System.IO.File.Exists(fullPath))
            {
                collisionCount++;
                var newFileName = $"{fileName[..fileName.LastIndexOf('.')]}({collisionCount}).{fileName.Split('.').Last()}";
                fullPath = Path.Combine(pathToSave, newFileName);
            }
            return fullPath;
        }

        private static string GetContentType(string contentType)
        {
            return contentType switch
            {
                "image/jpeg" => "Images",
                "image/png" => "Images",
                "image/bmp" => "Images",
                "image/gif" => "Images",
                "video/mp4" => "Videos",
                _ => "Forbidden",
            };
        }

        private static string SelectFolder()
        {
            Random rnd = new();
            return hexValues[rnd.Next(0, 15)];
        }

        private static readonly string[] hexValues = new string[]
        { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
    }
}
