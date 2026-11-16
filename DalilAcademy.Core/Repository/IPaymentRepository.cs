using DalilAcademy.Core.Entities;

namespace DalilAcademy.Core.Repository
{
    public interface IPaymentRepository
    {
        Task<Payment> CreateAsync(Payment payment);
        Task<Payment?> GetByPayPalOrderIdAsync(string payPalOrderId);
        Task UpdateAsync(Payment payment);
    }
}
