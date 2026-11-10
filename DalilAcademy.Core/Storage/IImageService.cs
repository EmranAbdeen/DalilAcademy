namespace DalilAcademy.Core.Storage
{
    public interface IImageService
    {
        Task<string> SaveCourseImageAsync(Stream content, string fileName, long length);
        void DeleteCourseImage(string? imageUrl);
    }
}
