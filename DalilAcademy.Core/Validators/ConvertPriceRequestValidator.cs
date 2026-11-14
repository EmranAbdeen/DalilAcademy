using FluentValidation;
using static DalilAcademy.Core.DTOs.Currency;

namespace DalilAcademy.Core.Validators
{
    public class ConvertPriceRequestValidator : AbstractValidator<ConvertPriceRequestDto>
    {
        public ConvertPriceRequestValidator()
        {
            RuleFor(x => x.FromCurrency).Matches("^[A-Z]{3}$");
            RuleFor(x => x.ToCurrency).Matches("^[A-Z]{3}$");
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}
