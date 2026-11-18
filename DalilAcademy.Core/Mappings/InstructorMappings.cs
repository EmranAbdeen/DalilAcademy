using DalilAcademy.Core.Entities;
using static DalilAcademy.Core.DTOs.InstructorDto;

namespace DalilAcademy.Core.Mappings
{
    public static class InstructorMappings
    {
        public static InstructorResponseDto ToDo(this Instructor instructor)
        {
            return new InstructorResponseDto(instructor.Id, instructor.FullName, instructor.Email);
        }
        public static void UpdateFromDto(this Instructor instructor, UpdateInstructorDto dto)
        {
            {
                instructor.FullName = dto.FullName;
                instructor.Email = dto.Email;
            };
        }
    }
}
