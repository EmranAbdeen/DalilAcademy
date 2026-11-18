namespace DalilAcademy.Core.DTOs
{
    public class InstructorDto
    {
        public record InstructorResponseDto(int Id, string FullName, string Email);
        public record UpdateInstructorDto(string FullName, string Email);
    }
}
