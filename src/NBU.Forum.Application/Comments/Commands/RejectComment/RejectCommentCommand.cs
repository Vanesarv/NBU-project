namespace NBU.Forum.Application.Comments.Commands.RejectComment;

using ErrorOr;
using MediatR;
using Contracts.Responses;

public sealed class RejectCommentCommand : IRequest<ErrorOr<RejectCommentResponse>>
{
    public RejectCommentCommand(string commentId)
    {
        this.CommentId = commentId;
    }

    public string CommentId { get; set; } = default!;
}
