namespace NBU.Forum.Infrastructure.Persistence.EntityConfigurations;

using Domain.CommentAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder
            .HasKey(k => k.Id);

        builder
            .HasOne(u => u.AppUser)
            .WithMany(c => c.Comments)
            .HasForeignKey(u => u.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(u => u.Article)
            .WithMany(c => c.Comments)
            .HasForeignKey(u => u.ArticleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
