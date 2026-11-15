using static DalilAcademy.Core.DTOs.CourseDto;
using static DalilAcademy.Core.DTOs.EnrollmentDto;
using static DalilAcademy.Core.DTOs.StudentDto;

namespace DalilAcademy.Core.Service
{
    public interface IEnrollmentService
    {
        Task<EnrollmentResponseDto> EnrollAsync(int studentId, int courseId);
        Task<EnrollmentResponseDto> UpdateEnrollmentAsync(int enrollmentId, int courseId);
        Task DeleteEnrollmentAsync(int enrollmentId);
        Task<List<CourseResponseDto>> GetMyCoursesAsync(int userId);
        Task<List<StudentResponseDto>> GetPaidStudentsByCourseIdAsync(int courseId);
    }
}
