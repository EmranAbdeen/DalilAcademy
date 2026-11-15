using DalilAcademy.Core.Entities;
using static DalilAcademy.Core.DTOs.CourseDto;

namespace DalilAcademy.Core.Repository
{
    public interface IEnrollmentRepository
    {
        Task<Enrollment> CreateAsync(Enrollment enrollment);
        Task<Enrollment?> GetByIdAsync(int id);
        Task UpdateAsync(Enrollment enrollment);
        Task DeleteAsync(Enrollment enrollment);
        Task<Student?> GetByUserIdAsync(int userId);
        Task<List<CourseResponseDto>> GetCoursesByStudentIdAsync(int studentId);
        Task<List<Student>> GetPaidStudentsByCourseIdAsync(int courseId);
    }
}
