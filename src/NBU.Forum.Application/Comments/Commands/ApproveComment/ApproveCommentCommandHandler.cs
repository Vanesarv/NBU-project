namespace NBU.Forum.Application.Comments.Commands.ApproveComment;

using ErrorOr;
using MediatR;
using Contracts.Responses;
using System.Threading;
using System.Threading.Tasks;

public sealed class ApproveCommentCommandHandler : IRequestHandler<ApproveCommentCommand,
    ErrorOr<ApproveCommentResponse>>
{
    private readonly ICommentRepository _commentRepository;

    public ApproveCommentCommandHandler(ICommentRepository commentRepository)
        => _commentRepository = commentRepository;

    public async Task<ErrorOr<ApproveCommentResponse>> Handle(ApproveCommentCommand request, CancellationToken cancellationToken)
    {
        var result = await _commentRepository.ApproveCommentAsync(request.CommentId,
            cancellationToken);

        return result.IsError
            ? result.Errors
            : new ApproveCommentResponse();
    }
}
