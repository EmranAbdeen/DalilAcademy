using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static DalilAcademy.Core.DTOs.CourseDto;
using static DalilAcademy.Core.DTOs.StudentDto;

namespace DalilAcademy.API.Controllers
{
    // zero try/catch blocks — errors go to GlobalExceptionHandler
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController(IStudentService studentService) : ControllerBase
    {
        // GetAll
        [HttpGet]
        public async Task<ActionResult<PagedResult<StudentResponseDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);
            return Ok(await studentService.GetPagedResult(page, pageSize));
        }

        // GetById
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<StudentResponseDto>> GetById(int id)
        {
            var student = await studentService.GetByIdAsync(id);
            return student is null ? NotFound() : Ok(student);
        }

        // profile
        [HttpGet]
        [Route("profile")]
        [Authorize(Roles = Roles.Student)]
        public async Task<ActionResult<StudentResponseDto>> GetProfile()
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(user, out var userId))
                return Unauthorized();

            var student = await studentService.GetProfileAsync(userId);

            return Ok(student);
        }

        //Create
        [HttpPost]
        public async Task<ActionResult<StudentResponseDto>> Create(CreateStudentDto createStudentDto)
        {
                var created = await studentService.CreateAsync(createStudentDto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // Update
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateStudentDto updateStudentDto)
        {
                await studentService.UpdateAsync(id, updateStudentDto);
                return NoContent();
        }

        //Delete
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await studentService.DeleteAsync(id);
                return NoContent();
        }

        [HttpGet]
        [Route("{id:int}/courses")]
        public async Task<ActionResult<List<CourseResponseDto>>> GetCourses(int id)
        {
            return Ok(await studentService.GetCoursesAsync(id));
        }
    }
}
