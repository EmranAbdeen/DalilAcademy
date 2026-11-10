using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using DalilAcademy.Core.Storage;
using FluentValidation;
using static DalilAcademy.Core.DTOs.CourseDto;
using static DalilAcademy.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Service
{
    public class CourseService(
        ICourseRepository courseRepository,
        IValidator<CreateCourseDto> createValidator,
        IValidator<UpdateCourseDto> updateValidator,
        IImageService images)
        : ICourseService
    {
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
        public async Task UpdateAsync(int id, UpdateCourseDto dto)
        {
            await updateValidator.ValidateAndThrowAsync(dto);

            var course = await courseRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Course {id} was not found.");

            var oldImage = course.ImageUrl;
            course.UpdateFromDto(dto);
            await courseRepository.UpdateAsync(course);                                 // duplicate title → 409, database first (saves inside)

            if (oldImage != dto.ImageUrl)
                images.DeleteCourseImage(oldImage);                                     // then the old file
        }
        public async Task DeleteAsync(int id)
        {
            var course = await courseRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Course {id} was not found.");

            await courseRepository.DeleteAsync(id);                                     // database first (saves inside)
            images.DeleteCourseImage(course.ImageUrl);                                  // then the file
        }
    }
}

