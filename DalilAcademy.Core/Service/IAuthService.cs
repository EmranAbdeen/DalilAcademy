using static DalilAcademy.Core.DTOs.Auth;

namespace DalilAcademy.Core.Service
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginRequestDto dto);
        Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task<CreateInstructorResponseDto> CreateInstructorAsync(CreateInstructorRequestDto dto);
    }
}
