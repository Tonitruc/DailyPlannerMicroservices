using UserService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UserService.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    private const int MaxUserNameLength = 50;


    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(e => e.Email)
            .IsRequired();

        builder.HasIndex(e => e.Email)
            .IsUnique();

        builder.Property(e => e.UserName)
            .HasMaxLength(MaxUserNameLength)
            .IsRequired();
    }
}
