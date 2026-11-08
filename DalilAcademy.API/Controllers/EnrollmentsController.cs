using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Mvc;
using static DalilAcademy.Core.DTOs.EnrollmentDto;

// only calls EnrollAsync — the business rule lives in the service
namespace DalilAcademy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController(IEnrollmentService enrollmentService) : ControllerBase
    {
        // 201 Created · 404 student/course not found · 409 already enrolled
        [HttpPost]
        public async Task<ActionResult<EnrollmentResponseDto>> Enroll(CreateEnrollmentDto dto)
        {
            var enrollment = await enrollmentService.EnrollAsync(dto.StudentId, dto.CourseId);

            return StatusCode(StatusCodes.Status201Created, enrollment);
        }
    }
}
