using DalilAcademy.Core.Entities;
using static DalilAcademy.Core.DTOs.StudentDto;

namespace DalilAcademy.Core.Mappings
{
    public static class StudentMappings
    {
        public static StudentResponseDto ToDto(this Student student)
        {
            return new StudentResponseDto(student.Id, student.FullName, student.Email);
        }
        public static Student ToEntity(this CreateStudentDto createStudentDto)
        {
            return new Student
            {
                FullName = createStudentDto.FullName,
                Email = createStudentDto.Email
            };
        }
        public static void UpdateFromDto(this Student student, UpdateStudentDto updateStudentDto)
        {
            {
                student.FullName = updateStudentDto.FullName;
                student.Email = updateStudentDto.Email;
            }
        }
    }
}
