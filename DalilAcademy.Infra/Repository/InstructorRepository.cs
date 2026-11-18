using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static DalilAcademy.Core.DTOs.InstructorDto;
using static DalilAcademy.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Repository
{
    public class InstructorRepository(AppDbContext db) : IInstructorRepository
    {
        public async Task<PagedResult<InstructorResponseDto>> GetPagedResult(int page, int pageSize)
        {
            var query = db.Instructors.AsNoTracking();
            var total = await query.CountAsync();

            var items = await query
                .OrderBy(i => i.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new InstructorResponseDto(
                    i.Id,
                    i.FullName,
                    i.Email
                ))
                .ToListAsync();

            return new PagedResult<InstructorResponseDto>(items, page, pageSize, total);
        }
        public async Task<Instructor?> GetByIdAsync(int id)
        {
            return await db.Instructors.Include(i => i.User).FirstOrDefaultAsync(i => i.Id == id);
        }
        public Task<bool> HasCoursesAsync(int instructorId)
        {
            return db.Courses.AnyAsync(c => c.InstructorId == instructorId);
        }
        public async Task AddAsync(Instructor instructor)
        {
            db.Instructors.Add(instructor);
            await db.SaveChangesAsync();
        }
        public async Task UpdateAsync(Instructor instructor)
        {
            try
            {
                db.Instructors.Update(instructor);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ConflictException("A instructor with this email or user already exists.");
            }
            catch (DbUpdateException ex)
            when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.ForeignKeyViolation })
            {
                throw new NotFoundException("User was not found.");
            }
        }
        public async Task DeleteAsync(Instructor instructor)
        {
            db.Instructors.Remove(instructor);
            await db.SaveChangesAsync();
        }
    }
}
