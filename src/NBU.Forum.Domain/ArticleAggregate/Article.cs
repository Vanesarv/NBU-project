namespace NBU.Forum.Domain.ArticleAggregate;

using AppUserAggregate;
using CommentAggregate;
using Shared;
using System;

public sealed class Article : Entity, IAuditEntity
{
    private Article(
        string title, 
        string content,
        string appUserId)
    {
        this.Title = title;
        this.Content = content;
        this.AppUserId = appUserId;
        this.Comments = new HashSet<Comment>();
    }

    public string Title { get; set; } = default!;

    public string Content { get; set; } = default!;

    public string AppUserId { get; set; }

    public AppUser AppUser { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime LastUpdatedAt { get; set; }

    public ICollection<Comment> Comments { get; set; }

    public static Article Create(
        string title,
        string content,
        string appUserId)
        => new(title,
            content,
            appUserId);
}
