using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Seedings;

internal static class Seeding
{
    public static void SeedUsers(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasData(new User 
            {
                Id = 1, 
                Name = "Admin",
                Email = "admin@backend.com",
                IsActive = true,
                Password= "admin",
                PhoneNumber = "123",
                Role = UserRole.Manager
            });
    }
}
