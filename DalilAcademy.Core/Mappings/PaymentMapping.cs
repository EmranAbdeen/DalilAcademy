using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Enums;
using static DalilAcademy.Core.DTOs.PaymentDto;

namespace DalilAcademy.Core.Mappings
{
    public static class PaymentMapping
    {
        public static Payment ToEntity(this CreatePaymentDto dto)
        {
            return new Payment
            {
                EnrollmentId = dto.EnrollmentId,
                Currency = "USD",
                Status = PaymentStatus.Created,
                CreatedAt = DateTime.UtcNow
            };
        }
        public static PaymentResponseDto ToDo(this Payment payment)
        {
            return new PaymentResponseDto(
                payment.Id,
                payment.EnrollmentId,
                payment.Amount,
                payment.Currency,
                payment.PayPalOrderId,
                payment.Status,
                payment.CreatedAt,
                payment.PaidAt
            );
        }
    }
}
