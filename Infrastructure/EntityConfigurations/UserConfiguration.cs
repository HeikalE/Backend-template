using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityConfigurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("user");

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .UseIdentityColumn();

        builder.Property(x => x.IsActive)
            .HasColumnName("isActive");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired();

        builder.Property(x => x.PhoneNumber)
            .HasColumnName("phone-number")
            .IsRequired();

        builder.Property(x => x.Email)
            .HasColumnName("email")
            .IsRequired();

        builder.HasIndex(r => r.Email)
            .IsUnique();

        builder.HasIndex(r => r.PhoneNumber)
            .IsUnique();

        builder.Property(x => x.Password)
            .HasColumnName("password")
            .IsRequired();
    }
}
