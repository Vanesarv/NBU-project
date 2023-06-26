namespace NBU.Forum.Domain.AppUserAggregate;

using ArticleAggregate;
using CommentAggregate;
using Microsoft.AspNetCore.Identity;

public sealed class AppUser : IdentityUser
{
    public AppUser()
    {
        this.Articles = new HashSet<Article>();
        this.Comments = new HashSet<Comment>();
    }

    public ICollection<Article> Articles { get; set; }

    public ICollection<Comment> Comments { get; set; }
}
