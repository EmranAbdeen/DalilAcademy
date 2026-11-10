namespace DalilAcademy.Core.DTOs
{
    public class CourseDto
    {
        public record CreateCourseDto(string Title, decimal price, int InstructorId, string? ImageUrl);
        public record UpdateCourseDto(string Title, decimal price, string? ImageUrl);
        public record CourseResponseDto(int Id, string Title, decimal price, string? InstructorName, string? ImageUrl);
    }
}