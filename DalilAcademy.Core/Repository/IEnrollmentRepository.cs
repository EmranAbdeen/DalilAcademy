using DalilAcademy.Core.Entities;
using static DalilAcademy.Core.DTOs.CourseDto;

namespace DalilAcademy.Core.Repository
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment> CreateAsync(Enrollment enrollment);
        Task<Student?> GetByUserIdAsync(int userId);
        Task<List<CourseResponseDto>> GetCoursesByStudentIdAsync(int studentId);
    }
}
