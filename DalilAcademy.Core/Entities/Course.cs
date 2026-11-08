using DalilAcademy.Core.Entities.Enums;

namespace DalilAcademy.Core.Entities
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public CourseLevel Level { get; set; } = CourseLevel.Beginner;
        public int InstructorId { get; set; }
        public Instructor? Instructor { get; set; }
        public List<Enrollment> Enrollments { get; set; } = [];

    }
}
