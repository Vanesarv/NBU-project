namespace NBU.Forum.Application.Comments.Commands.ApproveComment;

using ErrorOr;
using MediatR;
using Contracts.Responses;

public sealed class ApproveCommentCommand : IRequest<ErrorOr<ApproveCommentResponse>>
{
	public ApproveCommentCommand(string commentId)
	{
		this.CommentId = commentId;
	}

    public string CommentId { get; set; } = default!;
}
