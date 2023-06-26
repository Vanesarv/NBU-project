namespace NBU.Forum.Application.Comments.Commands.RejectComment;

using ErrorOr;
using MediatR;
using Contracts.Responses;

public sealed class RejectCommentCommandHandler : IRequestHandler<RejectCommentCommand,
    ErrorOr<RejectCommentResponse>>
{
    private readonly ICommentRepository _commentRepository;

    public RejectCommentCommandHandler(ICommentRepository commentRepository)
        => _commentRepository = commentRepository;

    public async Task<ErrorOr<RejectCommentResponse>> Handle(RejectCommentCommand request, CancellationToken cancellationToken)
    {
        var result = await _commentRepository.RejectCommentAsync(request.CommentId,
            cancellationToken);

        return result.IsError
            ? result.Errors
            : new RejectCommentResponse();
    }
}
