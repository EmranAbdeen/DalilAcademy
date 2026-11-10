using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static DalilAcademy.Core.DTOs.CourseDto;
using static DalilAcademy.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Repository
{
    public class EnrollmentRepository(AppDbContext db) : IEnrollmentRepository
    {
        public async Task<Enrollment> CreateAsync(Enrollment enrollment)
        {
            try
            {
                db.Enrollments.Add(enrollment);
                await db.SaveChangesAsync();

                return enrollment;
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                // unique index (StudentId, CourseId) → 409
                throw new ConflictException("This student is already enrolled in this course.");
            }
        }
        public async Task<Student?> GetByUserIdAsync(int userId)
        {
            return await db.Students.FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<List<CourseResponseDto>> GetCoursesByStudentIdAsync(int studentId)
        {
            return await db.Enrollments
                .Where(e => e.StudentId == studentId)
                .Include(e => e.Course)
                .Select(e => new CourseResponseDto(
                    e.Course!.Id,
                    e.Course.Title,
                    e.Course.Price,
                    e.Course.Instructor!.FullName,
                    e.Course.ImageUrl
                    ))
                .ToListAsync();
        }
    }
}
