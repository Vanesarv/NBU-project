namespace NBU.Forum.Application.Comments;

using ErrorOr;
using Domain.CommentAggregate;
using Contracts.Responses;

public interface ICommentRepository
{
    Task<IEnumerable<GetUnapprovedCommentResponse>> GetAsync(
        CancellationToken cancellationToken = default!);

    public Task<ErrorOr<Comment>> AddAsync(
        Comment comment,
        CancellationToken cancellationToken = default!);

    Task<ErrorOr<Comment>> ApproveCommentAsync(
        string commentId,
        CancellationToken cancellationToken = default!);

    Task<ErrorOr<Comment>> RejectCommentAsync(
        string commentId, 
        CancellationToken cancellationToken = default!);
}
