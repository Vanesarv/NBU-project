namespace NBU.Forum.Infrastructure.Persistence;

using Application.Services;
using Domain.Shared;
using Domain.AppUserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Domain.ArticleAggregate;
using Domain.CommentAggregate;

public sealed class ForumDbContext : IdentityDbContext<AppUser>
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public ForumDbContext(
        DbContextOptions<ForumDbContext> options,
        IDateTimeProvider dateTimeProvider) : base(options)
        => _dateTimeProvider = dateTimeProvider;

    public DbSet<Article> Articles { get; set; }

    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ForumDbContext).Assembly);

        base.OnModelCreating(builder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.AddAuditInfo();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void AddAuditInfo()
    {
        foreach (var entity in this.ChangeTracker.Entries<IAuditEntity>())
        {
            switch (entity.State)
            {
                case EntityState.Detached:
                    break;
                case EntityState.Unchanged:
                    break;
                case EntityState.Deleted:
                    break;
                case EntityState.Added:
                    entity.Entity.CreatedAt = _dateTimeProvider.UtcNow;
                    entity.Entity.LastUpdatedAt = _dateTimeProvider.UtcNow;
                    break;
                case EntityState.Modified:
                    entity.Entity.LastUpdatedAt = _dateTimeProvider.UtcNow;
                    break;
                default:
                    break;
            }
        }
    }
}
