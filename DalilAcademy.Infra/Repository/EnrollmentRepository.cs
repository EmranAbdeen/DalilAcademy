using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Enums;
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
        public async Task<Enrollment?> GetByIdAsync(int id)
        {
            return await db.Enrollments
                .Include(e => e.Student)
                .Include(e => e.Course)
                .ThenInclude(c => c!.Instructor)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task UpdateAsync(Enrollment enrollment)
        {
            try
            {
                db.Enrollments.Update(enrollment);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ConflictException("This student is already enrolled in this course.");
            }
        }

        public async Task DeleteAsync(Enrollment enrollment)
        {
            db.Enrollments.Remove(enrollment);
            await db.SaveChangesAsync();
        }

        public async Task<Student?> GetByUserIdAsync(int userId)
        {
            return await db.Students.FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task<List<CourseResponseDto>> GetCoursesByStudentIdAsync(int studentId)
        {
            return await db.Enrollments
                .Where(e => e.StudentId == studentId
                    && e.status == EnrollmentStatus.Paid)
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

        public async Task<List<Student>> GetPaidStudentsByCourseIdAsync(int courseId)
        {
            return await db.Enrollments
                .Where(e =>
                    e.CourseId == courseId && 
                    e.status == EnrollmentStatus.Paid)
                .Select(e => e.Student!)
                .ToListAsync();
        }
    }
}
