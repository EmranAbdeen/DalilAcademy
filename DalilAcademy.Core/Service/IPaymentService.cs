using static DalilAcademy.Core.DTOs.EnrollmentDto;
using static DalilAcademy.Core.DTOs.PaymentDto;

namespace DalilAcademy.Core.Service
{
    public interface IPaymentService
    {
        Task<PaymentResponseDto> PayAsync(int enrollmentId);
        Task<EnrollmentResponseDto> CaptureAsync(int enrollmentId);
    }
}
