namespace DalilAcademy.Core.DTOs
{
     public class PaymentDto
    {
        public record PaymentResponseDto(int PaymentId, string OrderId, decimal Amount, string Currency, string ApprovalUrl);
    }
}
