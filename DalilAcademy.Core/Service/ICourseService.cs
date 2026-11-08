using DalilAcademy.Core.DTOs;
using static DalilAcademy.Core.DTOs.CourseDto;

namespace DalilAcademy.Core.Service
{
    public interface ICourseService
    {
        //Task<List<CourseResponseDto>> GetAllAsync();
        Task<PagedResult<CourseResponseDto>> GetPageAsync(int page, int pageSize, decimal? minPrice, decimal? maxPrice);
        Task<CourseResponseDto?> GetByIdAsync(int id);
        Task<int> CountAsync();
        Task<CourseResponseDto> CreateAsync(CreateCourseDto createCourseDto);
        Task UpdateAsync(int id, UpdateCourseDto updateCourseDto);
        Task DeleteAsync(int id);
    }
}

