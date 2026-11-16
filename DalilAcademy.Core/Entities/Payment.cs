using DalilAcademy.Core.Enums;

namespace DalilAcademy.Core.Entities
{
    public class Payment
    {
        public int Id { get; set; }

        public int EnrollmentId { get; set; }
        public Enrollment? Enrollment { get; set; }

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "USD";
        public string PayPalOrderId { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
