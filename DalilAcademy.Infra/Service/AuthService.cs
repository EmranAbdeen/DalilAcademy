using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Repository;
using DalilAcademy.Core.Security;
using DalilAcademy.Core.Service;
using static DalilAcademy.Core.DTOs.Auth;
using static DalilAcademy.Core.Exceptions.AppExceptions;

namespace DalilAcademy.Infra.Service
{
    public class AuthService(IUserRepository users, IPasswordHasher hasher, ITokenService tokens) : IAuthService
    {
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await users.GetByEmailAsync(dto.Email);

            // Same message for "no such user" and "wrong password" — never reveal which
            if (user is null || !hasher.Verify(dto.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password."); 

            var (token, expiresAt) = tokens.CreateToken(user);
            return new LoginResponseDto(token, expiresAt);
        }

        public async Task RegisterAsync(RegisterRequestDto dto)
        {
            var user = await users.GetByEmailAsync(dto.Email);
            if (user is not null)
                throw new ConflictException("A user with this email already exists.");

            var passwordHash = hasher.Hash(dto.Password);
            var newUser = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = Roles.Student            // Default role is Student
            };
            await users.AddAsync(newUser);
        }
    }
}
