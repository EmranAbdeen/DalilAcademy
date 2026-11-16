using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Mappings;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using static DalilAcademy.Core.DTOs.PaymentDto;

namespace DalilAcademy.Infra.Service
{
    public class PaymentService(IPaymentRepository paymentRepository) : IPaymentService
    {
        public async Task<PaymentResponseDto> CreateAsync(CreatePaymentDto dto)
        {
            var payment = dto.ToEntity();

            var result = await paymentRepository.CreateAsync(payment);
            return result.ToDo();
        }
        public async Task<PaymentResponseDto?> GetByPayPalOrderIdAsync(string payPalOrderId)
        {
            var result = await paymentRepository.GetByPayPalOrderIdAsync(payPalOrderId);
            return result?.ToDo();
        }
        public async Task UpdateAsync(Payment payment)
        {
            await paymentRepository.UpdateAsync(payment);
        }
    }
}
