using DalilAcademy.Core.Enums;

namespace DalilAcademy.Core.Entities
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course? Course { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public DateTime EnrolledAt { get; set; }
        public EnrollmentStatus status { get; set; }
        public List<Payment> Payments { get; set; }
    }
}
