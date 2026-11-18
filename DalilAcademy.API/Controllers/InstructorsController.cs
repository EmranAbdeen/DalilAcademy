using DalilAcademy.Core.DTOs;
using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DalilAcademy.Core.DTOs.InstructorDto;

namespace DalilAcademy.API.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController(IInstructorService instructorService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedResult<InstructorResponseDto>>> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            page = Math.Max(page, 1);
            pageSize = Math.Clamp(pageSize, 1, 50);

            return Ok(await instructorService.GetPagedResult(page, pageSize));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<InstructorResponseDto>> GetById(int id)
        {
            var result = await instructorService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdateInstructorDto dto)
        {
            await instructorService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await instructorService.DeleteAsync(id);
            return NoContent();
        }
    }
}
