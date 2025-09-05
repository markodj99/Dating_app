using Microsoft.EntityFrameworkCore;
using API.DTO;
using System.Text.Json;
using System.Security.Cryptography;
using API.Model;
using System.Text;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(AppDbContext context)
        {
            if (await context.Users.AnyAsync()) return;

            var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<SeedUserDto>>(userData);
            if (users == null) return;

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                var usr = new User
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,
                    ImageUrl = user.ImageUrl,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes($"{user.Username.ToLower()}123")),
                    PasswordSalt = hmac.Key,
                    Member = new Member
                    {
                        Id = user.Id,
                        Username = user.Username,
                        Description = user.Description,
                        DateOfBirth = user.DateOfBirth,
                        ImageUrl = user.ImageUrl,
                        Gender = user.Gender,
                        City = user.City,
                        Country = user.Country,
                        LastActive = user.LastActive,
                        Created = user.Created,
                    }
                };

                usr.Member.Photos.Add(new Photo
                {
                    Url = user.ImageUrl!,
                    MemberId = usr.Member.Id,
                });

                context.Add(usr);
            }

            await context.SaveChangesAsync();
        }
    }
}
