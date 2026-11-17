using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Enums;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Service;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static DalilAcademy.Core.DTOs.EnrollmentDto;
using static DalilAcademy.Core.DTOs.PaymentDto;
using static DalilAcademy.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Service
{
    public class PaymentService(
        AppDbContext db,
        IPaymentRepository paymentRepository,
        IPaymentGateway paymentGateway,
        IConfiguration configuration) : IPaymentService
    {
        public async Task<PaymentResponseDto> PayAsync(int enrollmentId)
        {
            var enrollment = await db.Enrollments.Include(e => e.Course).FirstOrDefaultAsync(e => e.Id == enrollmentId);

            if (enrollment == null)
                throw new NotFoundException("Enrollment not found.");

            if (enrollment.status == EnrollmentStatus.Paid)
                throw new ConflictException("Enrollment is already paid.");

            // Get course price in JOD
            var amountInJod = enrollment.Course.Price;

            if (amountInJod <= 0)
                throw new ConflictException("This enrollment does not require payment.");

            // Convert JOD to USD
            var exchangeRate = configuration.GetValue<decimal>("PayPal:JodToUsdRate");

            if (exchangeRate <= 0)
                throw new InvalidOperationException("PayPal:JodToUsdRate must be greater than zero.");

            var amountInUsd = Math.Round(
                amountInJod * exchangeRate,
                2,
                MidpointRounding.AwayFromZero
            );

            // Create PayPal Order
            var order = await paymentGateway.CreateOrderAsync(amountInUsd);

            // Create Payment record
            var payment = new Payment
            {
                EnrollmentId = enrollmentId,
                Amount = amountInUsd,
                Currency = "USD",
                PayPalOrderId = order.orderId,
                Status = PaymentStatus.Created,
                CreatedAt = DateTime.UtcNow
            };

            await paymentRepository.CreateAsync(payment);

            return new PaymentResponseDto(
                payment.Id,
                order.orderId,
                payment.Amount,
                payment.Currency,
                order.ApprovalUrl
            );
        }

        public async Task<EnrollmentResponseDto> CaptureAsync(int enrollmentId)
        {
            var enrollment = await db.Enrollments
            .Include(e => e.Course)
            .Include(e => e.Student)
            .FirstOrDefaultAsync(e => e.Id == enrollmentId);

            if (enrollment == null)
                throw new NotFoundException("Enrollment not found.");

            if (enrollment.status == EnrollmentStatus.Paid)
                throw new ConflictException("Enrollment is already paid.");

            // Get latest Created Payment
            var payment = await paymentRepository.GetLatestPaymentAsync(enrollmentId);

            if (payment == null)
                throw new PaymentFailedException("Call /pay first to create a PayPal order.");

            // Check the 3-hour expiration
            if (DateTime.UtcNow - payment.CreatedAt > TimeSpan.FromHours(3))
            {
                payment.Status = PaymentStatus.Failed;
                await paymentRepository.UpdateAsync(payment);

                throw new PaymentFailedException("Payment order has expired. Please call /pay again.");
            }

            // Capture PayPal Order
            var isCompleted = await paymentGateway.CaptureOrderAsync(payment.PayPalOrderId);

            if (!isCompleted)
            {
                payment.Status = PaymentStatus.Failed;
                await paymentRepository.UpdateAsync(payment);

                throw new PaymentFailedException("PayPal payment was not completed. Please try again.");
            }

            // Update Payment
            payment.Status = PaymentStatus.Completed;
            payment.PaidAt = DateTime.UtcNow;

            // Update Enrollment
            enrollment.status = EnrollmentStatus.Paid;
            await paymentRepository.UpdateAsync(payment);

            return new EnrollmentResponseDto(
                enrollment.Id,
                enrollment.StudentId,
                enrollment.Student.FullName,
                enrollment.CourseId,
                enrollment.Course.Title,
                enrollment.EnrolledAt,
                enrollment.status
            );
        }
    }
}
