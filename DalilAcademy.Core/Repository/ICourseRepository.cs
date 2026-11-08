using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using static DalilAcademy.Core.DTOs.CourseDto;

namespace DalilAcademy.Core.Repository
{
    public interface ICourseRepository 
    {
        //Task<List<CourseResponseDto>> GetAllAsync();
        Task<PagedResult<CourseResponseDto>> GetPageAsync(int page, int pageSize, decimal? minPrice, decimal? maxPrice);
        Task<Course?> GetByIdAsync(int id);
        Task<int> CountAsync();
        Task<Course> CreateAsync(Course course);
        Task UpdateAsync(Course course);
        Task<bool> DeleteAsync(int id);
    }
}

