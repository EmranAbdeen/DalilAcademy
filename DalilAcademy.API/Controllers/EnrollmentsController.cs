using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static DalilAcademy.Core.DTOs.EnrollmentDto;

namespace DalilAcademy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentsController(IEnrollmentService enrollmentService, IPaymentService paymentService) : ControllerBase
    {
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<ActionResult<EnrollmentResponseDto>> Enroll(CreateEnrollmentDto dto)
        {
            var enrollment = await enrollmentService.EnrollAsync(dto.StudentId, dto.CourseId);

            return StatusCode(StatusCodes.Status201Created, enrollment);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEnrollmentDto dto)
        {
            var result = await enrollmentService.UpdateEnrollmentAsync(id, dto.CourseId);
            return Ok(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await enrollmentService.DeleteEnrollmentAsync(id);
            return NoContent();
        }

        [Authorize(Roles = Roles.Student)]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyCourses()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await enrollmentService.GetMyCoursesAsync(userId);

            return Ok(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("{id:int}/pay")]
        public async Task<IActionResult> Pay(int id)
        {
            var result = await paymentService.PayAsync(id);

            return Ok(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost("{id:int}/capture")]
        public async Task<IActionResult> Capture(int id)
        {
            var result = await paymentService.CaptureAsync(id);

            return Ok(result);
        }
    }
}
