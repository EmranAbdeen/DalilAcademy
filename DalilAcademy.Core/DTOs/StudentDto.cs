namespace DalilAcademy.Core.DTOs
{
    public class StudentDto
    {
        public record CreateStudentDto(string FullName, string Email, string PhoneNumber, string Password);
        public record UpdateStudentDto(string FullName, string PhoneNumber, string Email);
        public record StudentResponseDto(int Id, string FullName, string PhoneNumber, string Email);
    }
}
