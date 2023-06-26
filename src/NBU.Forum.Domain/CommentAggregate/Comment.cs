namespace NBU.Forum.Domain.CommentAggregate;

using Shared;
using AppUserAggregate;
using ArticleAggregate;

public sealed class Comment : Entity, IAuditEntity
{
    private Comment(
        string content,
        string articleId,
        string appUserId)
    {
        this.Content = content;
        this.ArticleId = articleId;
        this.AppUserId = appUserId;
    }

    public bool IsApproved { get; set; } = false;

    public string Content { get; set; } = default!;

    public bool Rejected { get; set; } = false;

    public string ArticleId { get; set; } = default!;

    public Article Article { get; set; }

    public string AppUserId { get; set; } = default!;

    public AppUser AppUser { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastUpdatedAt { get; set; }

    public void SetRejectedStatus()
        => this.Rejected = true;

    public void SetApprovedStatus()
        => this.IsApproved = true;

    public static Comment Create(
        string content,
        string articleId,
        string appUserId)
        => new(content,
            articleId,
            appUserId);
}
