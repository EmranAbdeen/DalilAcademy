namespace DalilAcademy.Core.DTOs
{
    public class PaymentGatewayDto
    {
        public record CreatePaymentOrderResult(string orderId, string ApprovalUrl);
    }
}
