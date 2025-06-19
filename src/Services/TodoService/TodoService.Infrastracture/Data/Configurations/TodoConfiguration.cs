using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoService.Domain.Models;

namespace TodoService.Infrastracture.Data.Configurations;

public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    private const int TitleMaxLength = 100;
    private const int DescriptionMaxLength = 500;


    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder
            .HasKey(todo => todo.Id);

        builder
            .Property(todo => todo.Title)
            .HasMaxLength(TitleMaxLength)
            .IsRequired();

        builder
            .Property(todo => todo.Description)
            .HasMaxLength(DescriptionMaxLength);

        builder.
            HasData(
                new Todo() {Id = 1, Title = "Test", Description = "Test", CreatedBy = "God"}
            );
    }
}
