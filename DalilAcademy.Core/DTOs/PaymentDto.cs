using DalilAcademy.Core.Enums;

namespace DalilAcademy.Core.DTOs
{
    public class PaymentDto
    {
        public record CreatePaymentDto(int EnrollmentId);
        public record PaymentResponseDto(
            int Id,
            int EnrollmentId,
            decimal Amount,
            string Currency,
            string PayPalOrderId,
            PaymentStatus Status,
            DateTime CreatedAt,
            DateTime? PaidAt
        );
    }
}
