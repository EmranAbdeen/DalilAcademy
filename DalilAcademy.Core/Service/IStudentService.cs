using DalilAcademy.Core.DTOs;
using static DalilAcademy.Core.DTOs.CourseDto;
using static DalilAcademy.Core.DTOs.StudentDto;

namespace DalilAcademy.Core.Service
{
    public interface IStudentService
    {
        Task<PagedResult<StudentResponseDto>> GetPagedResult(int page, int pageSize);
        Task<StudentResponseDto?> GetByIdAsync(int id);
        Task<List<CourseResponseDto>> GetProfileAsync(int id);
        Task<StudentResponseDto> CreateAsync(CreateStudentDto createStudentDto);
        Task UpdateAsync(int id, UpdateStudentDto updateStudentDto);
        Task DeleteAsync(int id);
        Task<List<CourseResponseDto>> GetCoursesAsync(int id);
    }
}
