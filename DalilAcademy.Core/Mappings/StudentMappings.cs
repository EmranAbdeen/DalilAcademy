using DalilAcademy.Core.Entities;
using static DalilAcademy.Core.DTOs.StudentDto;

namespace DalilAcademy.Core.Mappings
{
    public static class StudentMappings
    {
        public static StudentResponseDto ToDto(this Student student)
        {
            return new StudentResponseDto(student.Id, student.FullName,student.PhoneNumber, student.Email);
        }
        public static Student ToEntity(this CreateStudentDto createStudentDto)
        {
            return new Student
            {
                FullName = createStudentDto.FullName,
                PhoneNumber = createStudentDto.PhoneNumber,
                Email = createStudentDto.Email
            };
        }
        public static void UpdateFromDto(this Student student, UpdateStudentDto updateStudentDto)
        {
            {
                student.FullName = updateStudentDto.FullName;
                student.PhoneNumber = updateStudentDto.PhoneNumber;
                student.Email = updateStudentDto.Email;
            }
        }
    }
}
