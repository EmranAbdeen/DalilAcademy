using static DalilAcademy.Core.DTOs.Currency;

namespace DalilAcademy.Core.Service
{
    public interface ICurrencyService
    {
        Task<ConvertPriceResponseDto> ConvertAsync(ConvertPriceRequestDto requestDto);
    }
}
