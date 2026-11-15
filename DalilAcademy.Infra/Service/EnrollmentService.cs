using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Enums;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using static DalilAcademy.Core.DTOs.CourseDto;
using static DalilAcademy.Core.DTOs.EnrollmentDto;
using static DalilAcademy.Core.DTOs.StudentDto;
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
                EnrolledAt = DateTime.UtcNow,
                status = course.Price == 0 ? EnrollmentStatus.Paid : EnrollmentStatus.Pending
            };

            await enrollments.CreateAsync(enrollment);   // already enrolled → ConflictException → 409

            return enrollment.ToDto();
        }
        public async Task<EnrollmentResponseDto> UpdateEnrollmentAsync(int enrollmentId, int courseId)
        {
            var enrollment = await enrollments.GetByIdAsync(enrollmentId)
                ?? throw new NotFoundException($"Enrollment {enrollmentId} was not found.");

            if (enrollment.status == EnrollmentStatus.Paid)
            {
                throw new ConflictException("Cannot change the course of a paid enrollment.");
            }

            var course = await courses.GetByIdAsync(courseId)
                ?? throw new NotFoundException($"Course {courseId} was not found.");

            enrollment.CourseId = courseId;
            enrollment.Course = course;
            await enrollments.UpdateAsync(enrollment);

            return enrollment.ToDto();
        }
        public async Task DeleteEnrollmentAsync(int enrollmentId)
        {
            var enrollment = await enrollments.GetByIdAsync(enrollmentId)
                ?? throw new NotFoundException($"Enrollment {enrollmentId} was not found.");

            if (enrollment.status == EnrollmentStatus.Paid)
            {
                throw new ConflictException("Cannot delete a paid enrollment.");
            }

            await enrollments.DeleteAsync(enrollment);
        }

        public async Task<List<CourseResponseDto>> GetMyCoursesAsync(int userId)
        {
            var student = await students.GetByUserIdAsync(userId)
                ?? throw new NotFoundException("Student profile not found.");

            // Load student's enrolled courses
            return await enrollments.GetCoursesByStudentIdAsync(student.Id);
        }
        public async Task<List<StudentResponseDto>> GetPaidStudentsByCourseIdAsync(int courseId)
        {
            var course = await courses.GetByIdAsync(courseId)
                ?? throw new NotFoundException($"Course {courseId} was not found.");

            var studentsList = await enrollments.GetPaidStudentsByCourseIdAsync(courseId);
            return studentsList.Select(s => s.ToDto()).ToList();
        }
    }
}
