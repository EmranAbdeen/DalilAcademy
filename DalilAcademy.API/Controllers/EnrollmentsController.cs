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
    public class EnrollmentsController(IEnrollmentService enrollmentService) : ControllerBase
    {
        // 201 Created · 404 student/course not found · 409 already enrolled
        [HttpPost]
        public async Task<ActionResult<EnrollmentResponseDto>> Enroll(CreateEnrollmentDto dto)
        {
            var enrollment = await enrollmentService.EnrollAsync(dto.StudentId, dto.CourseId);

            return StatusCode(StatusCodes.Status201Created, enrollment);
        }

        [Authorize(Roles = Roles.Student)]
        [HttpGet("me")]
        public async Task<IActionResult> GetMyCourses()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await enrollmentService.GetMyCoursesAsync(userId);

            return Ok(result);
        }
    }
}
