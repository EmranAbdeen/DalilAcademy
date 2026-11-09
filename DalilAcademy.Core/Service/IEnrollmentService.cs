using static DalilAcademy.Core.DTOs.CourseDto;
using static DalilAcademy.Core.DTOs.EnrollmentDto;

namespace DalilAcademy.Core.Service
{
    public interface IEnrollmentService
    {
        Task<EnrollmentResponseDto> EnrollAsync(int studentId, int courseId);
        Task<List<CourseResponseDto>> GetMyCoursesAsync(int userId);
    }
}
