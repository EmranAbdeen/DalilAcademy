using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using static DalilAcademy.Core.DTOs.InstructorDto;

namespace DalilAcademy.Core.Repository
{
    public interface IInstructorRepository
    {
        //Task<List<Instructor>> GetAllAsync();
        Task<PagedResult<InstructorResponseDto>> GetPagedResult(int page, int pageSize);
        Task<Instructor?> GetByIdAsync(int id);
        Task<bool> HasCoursesAsync(int instructorId);
        Task AddAsync(Instructor instructor);
        Task UpdateAsync(Instructor instructor);
        Task DeleteAsync(Instructor instructor);
    }
}
