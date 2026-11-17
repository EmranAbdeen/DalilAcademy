using static DalilAcademy.Core.DTOs.PaymentGatewayDto;

namespace DalilAcademy.Core.Service
{
    public interface IPaymentGateway
    {
        Task<CreatePaymentOrderResult> CreateOrderAsync(decimal amountInUsd);
        Task<bool> CaptureOrderAsync(string orderId);
    }
}
