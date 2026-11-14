using DalilAcademy.Core.Service;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using static DalilAcademy.Core.DTOs.Currency;

namespace DalilAcademy.Infra.Service
{
    public class CurrencyService(HttpClient http) : ICurrencyService
    {
        private record PairResponse([property: JsonPropertyName("conversion_rate")] decimal ConversionRate);
        public async Task<ConvertPriceResponseDto> ConvertAsync(ConvertPriceRequestDto requestDto)
        {
            var data = await http.GetFromJsonAsync<PairResponse>($"pair/{requestDto.FromCurrency}/{requestDto.ToCurrency}") // https://v6.exchangerate-api.com/v6/YOUR-KEY/pair/JOD/USD
                ?? throw new HttpRequestException("Empty response from ExchangeRate-API.");

            var converted = Math.Round(requestDto.Amount * data.ConversionRate, 2);
            return new ConvertPriceResponseDto(requestDto.Amount, requestDto.FromCurrency, converted, requestDto.ToCurrency, data.ConversionRate);
        }
    }
}
