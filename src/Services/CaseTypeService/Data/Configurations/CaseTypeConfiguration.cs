using CaseTypeService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaseTypeService.Data.Configurations;

public class CaseTypeConfiguration : IEntityTypeConfiguration<CaseType>
{
    public void Configure(EntityTypeBuilder<CaseType> builder)
    {
        builder.HasIndex(ct => ct.Name)
            .IsUnique();

        builder.Property(ct => ct.Name)
            .HasMaxLength(50)
            .IsRequired();
    }
}
