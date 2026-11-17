using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Enums;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using static DalilAcademy.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Repository
{
    public class PaymentRepository(AppDbContext db) : IPaymentRepository
    {
        public async Task<Payment> CreateAsync(Payment payment)
        {
            try
            {
                await db.Payments.AddAsync(payment);
                await db.SaveChangesAsync();

                return payment;
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ConflictException("A payment with this PayPal order already exists.");
            }
        }
        public async Task<Payment?> GetByPayPalOrderIdAsync(string orderId)
        {
            return await db.Payments.FirstOrDefaultAsync(p => p.PayPalOrderId == orderId);
        }
        public async Task UpdateAsync(Payment payment)
        {
            try
            {
                db.Payments.Update(payment);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
                when (ex.InnerException is PostgresException { SqlState: PostgresErrorCodes.UniqueViolation })
            {
                throw new ConflictException("A payment with this PayPal order already exists.");
            }
        }
        public async Task<Payment?> GetLatestPaymentAsync(int enrollmentId)
        {
            return await db.Payments
                .Where(p =>
                    p.EnrollmentId == enrollmentId &&
                    p.Status == PaymentStatus.Created)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
