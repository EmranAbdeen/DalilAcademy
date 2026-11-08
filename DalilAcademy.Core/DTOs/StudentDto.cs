namespace DalilAcademy.Core.DTOs
{
    public class StudentDto
    {
        public record CreateStudentDto(string FullName, string Email, string Password);
        public record UpdateStudentDto(string FullName, string Email);
        public record StudentResponseDto(int Id, string FullName, string Email);
    }
}
