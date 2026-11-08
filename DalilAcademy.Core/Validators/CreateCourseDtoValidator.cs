using FluentValidation;
using static DalilAcademy.Core.DTOs.CourseDto;

namespace DalilAcademy.Core.Validators
{
    public class CreateCourseDtoValidator : AbstractValidator<CreateCourseDto>
    {
        public CreateCourseDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.price).GreaterThanOrEqualTo(0).WithMessage("Price must be zero or more.");
            RuleFor(x => x.InstructorId).GreaterThan(0);
        }
    }
}
