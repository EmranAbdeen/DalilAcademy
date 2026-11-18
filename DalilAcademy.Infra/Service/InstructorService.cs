using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using static DalilAcademy.Core.DTOs.InstructorDto;
using static DalilAcademy.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Service
{
    public class InstructorService(IInstructorRepository instructorRepository) : IInstructorService
    {
        //public async Task<List<InstructorResponseDto>> GetAllAsync()
        //{
        //    var instructor = await instructorRepository.GetAllAsync();
        //    return instructor.Select(i => i.ToDo()).ToList();
        //}
        public Task<PagedResult<InstructorResponseDto>> GetPagedResult(int page, int pageSize)
        {
            return instructorRepository.GetPagedResult(page, pageSize);
        }

        public async Task<InstructorResponseDto> GetByIdAsync(int id) 
        {
            var instructor = await instructorRepository.GetByIdAsync(id);
            if (instructor is null)
                throw new NotFoundException($"Instructor {id} was not found.");

            return instructor.ToDo();
        }
        public async Task UpdateAsync(int id, UpdateInstructorDto dto)
        {
            var instructor = await instructorRepository.GetByIdAsync(id);
            if (instructor is null)
                throw new NotFoundException($"Instructor {id} was not found.");

            instructor.UpdateFromDto(dto);

            await instructorRepository.UpdateAsync(instructor);
        }
        public async Task DeleteAsync(int id)
        {
            var instructor = await instructorRepository.GetByIdAsync(id);
            if (instructor is null)
                throw new NotFoundException($"Instructor {id} was not found.");

            if (await instructorRepository.HasCoursesAsync(id))
                throw new ConflictException("Cannot delete an instructor who has courses. Delete or reassign the courses first.");

            await instructorRepository.DeleteAsync(instructor);
        }
    }
}
