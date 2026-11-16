using DalilAcademy.Core.Entities;
using static DalilAcademy.Core.DTOs.PaymentDto;

namespace DalilAcademy.Core.Service
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> CreateAsync(CreatePaymentDto dto);
        Task<PaymentResponseDto?> GetByPayPalOrderIdAsync(string payPalOrderId);
        Task UpdateAsync(Payment payment);
    }
}
