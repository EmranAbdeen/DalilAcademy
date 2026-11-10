using FluentValidation;
using static DalilAcademy.Core.DTOs.CourseDto;

namespace DalilAcademy.Core.Validators
{
    public class UpdateCourseDtoValidator : AbstractValidator<UpdateCourseDto>
    {
        public UpdateCourseDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
            RuleFor(x => x.price).GreaterThanOrEqualTo(0).WithMessage("Price must be zero or more.");
            RuleFor(x => x.ImageUrl)
                .Must(url => url is null || (url.StartsWith("/images/courses/") && !url.Contains("..")))
                .WithMessage("ImageUrl must come from the upload endpoint.");
        }
    }
}
