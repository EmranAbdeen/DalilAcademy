using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using DalilAcademy.Infra.Repository;
using static DalilAcademy.Core.DTOs.CourseDto;
using static DalilAcademy.Core.DTOs.EnrollmentDto;
using static DalilAcademy.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Service
{
    public class EnrollmentService(
        IStudentRepository students,
        ICourseRepository courses,
        IEnrollmentRepository enrollments)
        : IEnrollmentService
    {
        public async Task<EnrollmentResponseDto> EnrollAsync(int studentId, int courseId)
        {
            var student = await students.GetByIdAsync(studentId)
                ?? throw new NotFoundException($"Student {studentId} was not found.");  //  404

            var course = await courses.GetByIdAsync(courseId)
                ?? throw new NotFoundException($"Course {courseId} was not found.");    // 404

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                Student = student,
                CourseId = courseId,
                Course = course,
                EnrolledAt = DateTime.UtcNow
            };

            await enrollments.CreateAsync(enrollment);   // already enrolled → ConflictException → 409

            return enrollment.ToDto();
        }
        public async Task<List<CourseResponseDto>> GetMyCoursesAsync(int userId)
        {
            var student = await students.GetByUserIdAsync(userId)
                ?? throw new NotFoundException("Student profile not found.");

            // Load student's enrolled courses
            return await enrollments.GetCoursesByStudentIdAsync(student.Id);
        }
    }
}
