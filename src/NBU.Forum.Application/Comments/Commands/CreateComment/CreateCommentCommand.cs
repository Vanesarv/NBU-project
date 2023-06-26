namespace NBU.Forum.Application.Comments.Commands.CreateComment;

using ErrorOr;
using MediatR;
using Contracts.Responses;

public sealed class CreateCommentCommand : IRequest<ErrorOr<CreateCommentResponse>>
{
    public CreateCommentCommand(string articleId, string appUserId, string content)
    {
        ArticleId = articleId;
        AppUserId = appUserId;
        Content = content;
    }

    public string ArticleId { get; set; } = default!;

    public string AppUserId { get; set; } = default!;

    public string Content { get; set; } = default!;
}
