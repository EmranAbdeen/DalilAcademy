using DalilAcademy.Core.Entities;
using static DalilAcademy.Core.DTOs.EnrollmentDto;

namespace DalilAcademy.Core.Mappings
{
    public static class EnrollmentMappings
    {
        public static EnrollmentResponseDto ToDto(this Enrollment enrollment)
        {
            return new EnrollmentResponseDto(
                enrollment.Id,
                enrollment.StudentId,
                enrollment.Student?.FullName ?? string.Empty,
                enrollment.CourseId,
                enrollment.Course?.Title ?? string.Empty,
                enrollment.EnrolledAt
            );
        }
    }
}
