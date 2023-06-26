namespace NBU.Forum.Application.Articles.Commands.CreateArticle;

using MediatR;
using ErrorOr;
using Contracts.Responses;

public sealed class CreateArticleCommand : IRequest<ErrorOr<CreateArticleResponse>>
{
    public CreateArticleCommand(
        string title,
        string content,
        string appUserId)
    {
        Title = title;
        Content = content;
        AppUserId = appUserId;
    }

    public string Title { get; set; } = default!;

    public string Content { get; set; } = default!;

    public string AppUserId { get; set; } = default!;
}