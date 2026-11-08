using FluentValidation;
using static DalilAcademy.Core.DTOs.StudentDto;

namespace DalilAcademy.Core.Validators
{
    public class CreateStudentDtoValidator : AbstractValidator<CreateStudentDto>
    {
        public CreateStudentDtoValidator()
        {
            RuleFor(x => x.FullName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(255);
        }
    }
}
