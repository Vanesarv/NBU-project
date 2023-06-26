namespace NBU.Forum.Infrastructure.Persistence.EntityConfigurations;

using Domain.ArticleAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(k => k.Id);

        builder
            .HasMany(c => c.Comments)
            .WithOne(a => a.Article)
            .HasForeignKey(a => a.ArticleId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(u => u.AppUser)
            .WithMany(a => a.Articles)
            .HasForeignKey(u => u.AppUserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
