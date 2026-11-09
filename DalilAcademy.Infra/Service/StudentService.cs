using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Security;
using DalilAcademy.Core.Service;
using FluentValidation;
using static DalilAcademy.Core.DTOs.CourseDto;
using static DalilAcademy.Core.DTOs.StudentDto;
using static DalilAcademy.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Service
{
    public class StudentService(
        IStudentRepository studentRepository,
        IValidator<CreateStudentDto> createValidator,
        IValidator<UpdateStudentDto> updateValidator,
        IPasswordHasher passwordHasher) 
        : IStudentService
    {
        public async Task<PagedResult<StudentResponseDto>> GetPagedResult(int page, int pageSize)
        {
            return await studentRepository.GetPagedResult(page, pageSize);
            //return student.Select(s => s.ToDto()).ToList();
        }
        public async Task<StudentResponseDto?> GetByIdAsync(int id)
        {
            var student = await studentRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Student {id} was not found.");
            return student?.ToDto();
        }
        public async Task<StudentResponseDto> GetMeAsync(int id)
        {
            var student = await studentRepository.GetByUserIdAsync(id)
                ?? throw new NotFoundException("Student profile was not found.");

            return student.ToDto();
        }
        public async Task<StudentResponseDto> CreateAsync(CreateStudentDto createStudentDto)
        {
            await createValidator.ValidateAndThrowAsync(createStudentDto);                  // 400

            var student = createStudentDto.ToEntity();
            student.User = new User
            {
                Email = createStudentDto.Email,
                PasswordHash = passwordHasher.Hash(createStudentDto.Password)
            };

            var created = await studentRepository.CreateAsync(student);                     // duplicate email → 409

            return created.ToDto();
        }
        public async Task UpdateAsync(int id, UpdateStudentDto updateStudentDto)
        {
            await updateValidator.ValidateAndThrowAsync(updateStudentDto);                  // 400

            var student = await studentRepository.GetByIdAsync(id)
                ?? throw new NotFoundException($"Student {id} was not found.");

            student.UpdateFromDto(updateStudentDto);
            await studentRepository.UpdateAsync(student);                                   // duplicate email → 409
        }
        public async Task DeleteAsync(int id)
        {
            if (!await studentRepository.DeleteAsync(id))
                throw new NotFoundException($"Student {id} was not found.");
        }
        public async Task<List<CourseResponseDto>> GetCoursesAsync(int id)
        {
            var student = await studentRepository.GetWithCoursesAsync(id)
                ?? throw new NotFoundException($"Student {id} was not found.");

            return student.Enrollments
                .Where(e => e.Course is not null)
                .Select(e => e.Course!.ToDto()).ToList();
        }
    }
}
