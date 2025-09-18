using API.DTO;
using API.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.Data;

public class Seed
{
    public static async Task SeedUsers(UserManager<User> _userManager)
    {
        if (await _userManager.Users.AnyAsync()) return;

        var memberData = await File.ReadAllTextAsync("Data/UserSeedData.json");
        var members = JsonSerializer.Deserialize<List<SeedUserDto>>(memberData);

        if (members == null)
        {
            Console.WriteLine("No members in seed data.");
            return;
        }

        foreach (var member in members)
        {
            var user = new User
            {
                Id = member.Id,
                Email = member.Email,
                UserName = member.UserName,
                ImageUrl = member.ImageUrl,
                Member = new Member
                {
                    Id = member.Id,
                    UserName = member.UserName,
                    Description = member.Description,
                    DateOfBirth = member.DateOfBirth,
                    ImageUrl = member.ImageUrl,
                    Gender = member.Gender,
                    City = member.City,
                    Country = member.Country,
                    LastActive = member.LastActive,
                    Created = member.Created
                }
            };

            user.Member.Photos.Add(new Photo
            {
                Url = member.ImageUrl!,
                MemberId = member.Id,
                IsApproved = true
            });

            var result = await _userManager.CreateAsync(user, $"{user.UserName.ToLower()}123");
            if (!result.Succeeded)
            {
                Console.WriteLine(result.Errors.First().Description);
            }
            await _userManager.AddToRoleAsync(user, "Member");
        }

        var admin = new User
        {
            Id = "admin-main-id",
            UserName = "Admin",
            Email = "admin@test.com",
            ImageUrl = "https://randomuser.me/api/portraits/men/19.jpg"
        };

        var adminResult = await _userManager.CreateAsync(admin, "admin123");
        if (!adminResult.Succeeded)
        {
            Console.WriteLine(adminResult.Errors.First().Description);
        }
        await _userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
    }
}