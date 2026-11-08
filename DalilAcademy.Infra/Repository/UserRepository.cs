using DalilAcademy.Core.Entities;
using DalilAcademy.Core.Repository;
using DalilAcademy.Infra.Data;
using Microsoft.EntityFrameworkCore;

namespace DalilAcademy.Infra.Repository
{
    public class UserRepository(AppDbContext db) : IUserRepository
    {
        public Task<User?> GetByEmailAsync(string email)
        {
            return db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        /*
        * 1- Find the user
        * var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
        * 2- Check Pass
        * if(user is null || !BCrypt.Net.BCrypt.Verfiy(Password, PasswordHash))
        * return Unauthorized();
        * 3- Generate Token
        * var token = // Create token
        * return Ok(token)
        */

        public Task AddAsync(User user)
        {
            db.Users.Add(user);
            return db.SaveChangesAsync();
        }
    }
}
