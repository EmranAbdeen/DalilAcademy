using DalilAcademy.Core.Entities;
using static DalilAcademy.Core.DTOs.CourseDto;

namespace DalilAcademy.Core.Mappings
{
    public static class CourseMappings
    {
        public static CourseResponseDto ToDto(this Course course) // Using `this` makes a method an extension method, such as those available in `Microsoft.EntityFrameworkCore`
        {
            return new CourseResponseDto(course.Id, course.Title, course.Price, course.Level, course.Instructor?.FullName);
        }

        public static Course ToEntity(this CreateCourseDto dto)
        {
            return new Course
            {
                Title = dto.Title,
                Price = dto.price,
                Level =dto.Level,
                InstructorId = dto.InstructorId
            };
        }

        public static void UpdateFromDto(this Course course, UpdateCourseDto dto)
        {
            {
                course.Title = dto.Title;
                course.Price = dto.price;
                course.Level = dto.Level;
            }
        }
    }
}
