using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using static DalilAcademy.Core.DTOs.StudentDto;

namespace DalilAcademy.Core.Repository
{
    public interface IStudentRepository
    {
        Task<PagedResult<StudentResponseDto>> GetPagedResult(int page, int pageSize);
        Task<Student?> GetByIdAsync(int id);
        Task<Student?> GetByUserIdAsync(int userId);
        Task AddAsync(Student student);
        Task<Student?> GetWithCoursesAsync(int id);
        Task<Student> CreateAsync(Student student);
        Task UpdateAsync(Student student);
        Task<bool> DeleteAsync(int id);
    }
}
