using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DalilAcademy.Core.DTOs.CourseDto;
using static DalilAcademy.Core.Exceptions.AppExceptions;

namespace DalilAcademy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CoursesController(ICourseService courseService) : ControllerBase
    {
        // Inject Service Layer (CourseService)
        //================================================================

        // Primary Constructor (the modern approach):
        //CoursesController(ICourseService courseService)

        // Traditional Constructor (The traditional method):
        //private readonly ICourseService _courseService;
        //public CoursesController(ICourseService courseService)
        //{
        //    _courseService = courseService;
        //}

        //================================================================


        // GetAll
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<CourseResponseDto>>> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);                         // from 1-50 only
            return Ok(await courseService.GetPageAsync(page, pageSize, minPrice, maxPrice));
        }

        // GetById
        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<CourseResponseDto>> GetById(int id)
        {
            var course = await courseService.GetByIdAsync(id);
            return course is null ? NotFound() : Ok(course);
        }

        // Count
        [HttpGet]
        [Route("count")]
        [AllowAnonymous]
        public async Task<ActionResult<int>> Count()
        {
            return Ok(await courseService.CountAsync());
        }
        //Create
        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult<CourseResponseDto>> Create(CreateCourseDto createCourseDto)
        {
                var createdCourse = await courseService.CreateAsync(createCourseDto);
                return CreatedAtAction(nameof(GetById), new { id = createdCourse.Id }, createdCourse);
        }

        // Update
        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = $"{Roles.Admin},{Roles.Instructor}")]
        public async Task<IActionResult> Update(int id, UpdateCourseDto updateCourseDto)
        {
            await courseService.UpdateAsync(id, updateCourseDto);
            return NoContent();
        }

        //Delete
        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            await courseService.DeleteAsync(id);
            return NoContent();
        }
    }
}

