using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static DalilAcademy.Core.DTOs.Auth;

namespace DalilAcademy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto dto)
        {
            var result = await authService.LoginAsync(dto);
            return Ok(result);
        }
        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterRequestDto dto)
        {
            var result = await authService.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost]
        [Route("instructors")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<ActionResult> CreateInstructor(CreateInstructorRequestDto dto)
        {
            var result = await authService.CreateInstructorAsync(dto);
            return Ok(result);
        }
    }
}
