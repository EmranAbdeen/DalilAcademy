using DalilAcademy.Core.Entities.Enums;

namespace DalilAcademy.Core.DTOs
{
    public class CourseDto
    {
        public record CreateCourseDto(string Title, decimal price, CourseLevel Level, int InstructorId);
        public record UpdateCourseDto(string Title, decimal price, CourseLevel Level);
        public record CourseResponseDto(int Id, string Title, decimal price, CourseLevel Level, string? InstructorName);
    }
}