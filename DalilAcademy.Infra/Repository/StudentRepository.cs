using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static DalilAcademy.Core.DTOs.StudentDto;
using static DalilAcademy.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Repository
{
    public class StudentRepository(AppDbContext db) : IStudentRepository
    {
        public async Task<PagedResult<StudentResponseDto>> GetPagedResult(int page, int pageSize)
        {
            var query = db.Students.AsNoTracking();
            var total = await query.CountAsync();
            var items = await query
                .OrderBy(s => s.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new StudentResponseDto(s.Id, s.FullName,s.PhoneNumber, s.Email)).ToListAsync();

            return new PagedResult<StudentResponseDto>(items, page, pageSize, total);
        }
        public async Task<Student?> GetByIdAsync(int id)
        {
            return await db.Students.FindAsync(id);
        }
        public async Task<Student?> GetByUserIdAsync(int userId)
        {
            return await db.Students.FirstOrDefaultAsync(s => s.UserId == userId);
        }
        public async Task AddAsync(Student student)
        {
            db.Students.Add(student);
            await db.SaveChangesAsync();
        }
        public async Task<Student?> GetWithCoursesAsync(int id)
        {
            return await db.Students
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                        .ThenInclude(c => c!.Instructor)
                            .FirstOrDefaultAsync(s => s.Id == id);
        }
        public async Task<Student> CreateAsync(Student student)
        {
            try
            {
                db.Students.Add(student);
                await db.SaveChangesAsync();
                return student;
            }
            catch(DbUpdateException ex)
                when(ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ConflictException("A student with this email or user already exists.");
            }
        }
        public async Task UpdateAsync(Student student)
        {
            try
            {
                db.Students.Update(student);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
               when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ConflictException("A student with this email already exists.");
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var student = await db.Students.FindAsync(id);
            if (student is null) return false;

            db.Students.Remove(student);
            await db.SaveChangesAsync();

            return true;
        }
    }
}
