using DalilAcademy.Core.Storage;
using static DalilAcademy.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Storage
{
    public class ImageService : IImageService
    {
        private readonly string _folder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "courses");
        private static readonly string[] Allowed = [".jpg", ".jpeg", ".png", ".webp"];

        public async Task<string> SaveCourseImageAsync(Stream content, string fileName, long length)
        {
            if (length == 0 || length > 2 * 1024 * 1024)
                throw new InvalidFileException("Image must be less than 2 MB.");

            var ext = Path.GetExtension(fileName).ToLower();
            if (!Allowed.Contains(ext))
                throw new InvalidFileException("Only jpg, jpeg, png and webp are allowed.");

            Directory.CreateDirectory(_folder);
            var name = $"{Guid.NewGuid()}{ext}";
            await using var file = File.Create(Path.Combine(_folder, name));
            await content.CopyToAsync(file);

            // Return the relative path to the saved image
            return $"/images/courses/{name}";
        }

        public void DeleteCourseImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return;

            var path = Path.Combine(_folder, Path.GetFileName(imageUrl));
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
