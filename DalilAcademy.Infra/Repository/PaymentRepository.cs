using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace DalilAcademy.Infra.Repository
{
    public class PaymentRepository(AppDbContext db) : IPaymentRepository
    {
        public async Task<Payment> CreateAsync(Payment payment)
        {
            await db.Payments.AddAsync(payment);
            await db.SaveChangesAsync();

            return payment;
        }
        public async Task<Payment?> GetByPayPalOrderIdAsync(string payPalOrderId)
        {
            return await db.Payments.FirstOrDefaultAsync(p => p.PayPalOrderId == payPalOrderId);
        }
        public async Task UpdateAsync(Payment payment)
        {
            db.Payments.Update(payment);
            await db.SaveChangesAsync();
        }
    }
}
