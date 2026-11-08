using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using FluentValidation;
using static DalilAcademy.Core.DTOs.CourseDto;
using static DalilAcademy.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Service
{
    public class CourseService(
        ICourseRepository courseRepository,
        IValidator<CreateCourseDto> createValidator,
        IValidator<UpdateCourseDto> updateValidator)
        : ICourseService
    {
        //public Task<List<CourseResponseDto>> GetAllAsync()
        //{
        //    return courseRepository.GetAllAsync();
        //    //return course.Select(c => c.ToDto()).ToList();
        //}
        public Task<PagedResult<CourseResponseDto>> GetPageAsync(int page, int pageSize, decimal? minPrice, decimal? maxPrice)
        {
            return courseRepository.GetPageAsync(page, pageSize, minPrice, maxPrice);
        }
        public async Task<CourseResponseDto?> GetByIdAsync(int id)
        {
            var course = await courseRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Course {id} was not found.");
            return course?.ToDto();
        }
        public Task<int> CountAsync()
        {
            return courseRepository.CountAsync();
        }
        public async Task<CourseResponseDto> CreateAsync(CreateCourseDto createCourseDto)
        {
            await createValidator.ValidateAndThrowAsync(createCourseDto);               // 400

            var course = createCourseDto.ToEntity();
            var created = await courseRepository.CreateAsync(course);                   // duplicate title → 409

            return created.ToDto();
        }
        public async Task UpdateAsync(int id, UpdateCourseDto updateCourseDto)
        {
            await updateValidator.ValidateAndThrowAsync(updateCourseDto);

            var course = await courseRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Course {id} was not found.");

            course.UpdateFromDto(updateCourseDto);
            await courseRepository.UpdateAsync(course);                                 // duplicate title → 409
        }
        public async Task DeleteAsync(int id)
        {
            if(!await courseRepository.DeleteAsync(id))
                throw new NotFoundException($"Course {id} was not found.");
        }
    }
}

