using TodoTypeService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TodoTypeService.Data.Configurations;

public class TodoTypeConfiguration : IEntityTypeConfiguration<TodoType>
{
    public void Configure(EntityTypeBuilder<TodoType> builder)
    {
        builder.HasIndex(ct => ct.Name)
            .IsUnique();

        builder.Property(ct => ct.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}
