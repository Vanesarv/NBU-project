namespace NBU.Forum.Application.Comments.Queries.GetUnapprovedComments;

using MediatR;
using Contracts.Responses;

public sealed class GetUnapprovedCommentsQueryHandler : IRequestHandler<GetUnapprovedCommentsQuery, IEnumerable<GetUnapprovedCommentResponse>>
{
    private readonly ICommentRepository _commentRepository;

    public GetUnapprovedCommentsQueryHandler(ICommentRepository commentRepository)
        => _commentRepository = commentRepository;

    public async Task<IEnumerable<GetUnapprovedCommentResponse>> Handle(GetUnapprovedCommentsQuery request, CancellationToken cancellationToken)
    {
        var result = await _commentRepository.GetAsync(cancellationToken);

        return result;
    }
}
