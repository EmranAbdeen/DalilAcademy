using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static DalilAcademy.Core.DTOs.CourseDto;
using static DalilAcademy.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Repository
{
    public class CourseRepository(AppDbContext db) : ICourseRepository
    {
        //public Task<List<CourseResponseDto>> GetAllAsync()
        //{
        //    return await db.Courses.Include(c => c.Instructor).ToListAsync();
              //return db.Courses
              //  .AsNoTracking()                         // read-only: skip change tracking
              //  .Select(c => new CourseResponseDto(
              //      c.Id,
              //      c.Title,
              //      c.Price,
              //      c.Level,
              //      c.Instructor!.FullName))            // EF writes the JOIN for you
              //  .ToListAsync();
        //}
        public async Task<PagedResult<CourseResponseDto>> GetPageAsync(int page, int pageSize, decimal? minPrice, decimal? maxPrice)
        {
            var query = db.Courses.AsNoTracking();
            var total = await query.CountAsync();

            // Query: In SQL Server, filtering takes place before data is retrieved
            // Items: This occurs in memory after the data has been fetched, which impacts performance.
                // minPrice, maxPrice

            var items = await query
                .Where(c =>
                    (!minPrice.HasValue || c.Price >= minPrice.Value) &&
                    (!maxPrice.HasValue || c.Price <= maxPrice.Value))
                .OrderBy(c => c.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CourseResponseDto(c.Id, c.Title, c.Price, c.Instructor!.FullName))
                .ToListAsync();

            return new PagedResult<CourseResponseDto>(items, page, pageSize, total);
        }
        public async Task<Course?> GetByIdAsync(int id)
        {
            return await db.Courses.Include(c => c.Instructor).FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<int> CountAsync()
        {
            return await db.Courses.CountAsync();
        }
        public async Task<Course> CreateAsync(Course course)
        {
            try
            {
                db.Courses.Add(course);
                await db.SaveChangesAsync();

                // Load the instructor so the response can show InstructorName
                await db.Entry(course).Reference(c => c.Instructor).LoadAsync();

                return course;
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation }) // Exception Filter
            {
                throw new ConflictException("A course with this title already exists.");
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.ForeignKeyViolation }) // Exception Filter
            {
                throw new NotFoundException($"Instructor {course.InstructorId} was not found.");
            }
        }
        public async Task UpdateAsync(Course course)
        {
            try
            {
                db.Courses.Update(course);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation }) // Exception Filter
            {
                throw new ConflictException("A course with this title already exists.");
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingCourse = await db.Courses.FindAsync(id);
            if (existingCourse == null)
                return false;

            db.Courses.Remove(existingCourse);
            await db.SaveChangesAsync();

            return true;
        }
    }
}

