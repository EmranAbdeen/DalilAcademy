namespace DalilAcademy.Core.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public User? User { get; set; }
        public List<Enrollment> Enrollments { get; set; } = [];
    }
}
