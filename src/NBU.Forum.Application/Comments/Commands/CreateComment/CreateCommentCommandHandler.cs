namespace NBU.Forum.Application.Comments.Commands.CreateComment;

using ErrorOr;
using MediatR;
using Contracts.Responses;
using Domain.CommentAggregate;
using Application.Prediction;

public sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand,
    ErrorOr<CreateCommentResponse>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPredictionService _predictionService;

    public CreateCommentCommandHandler(
        ICommentRepository commentRepository,
        IPredictionService predictionService)
        => (_commentRepository, _predictionService) = (commentRepository, predictionService);

    public async Task<ErrorOr<CreateCommentResponse>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = Comment.Create(
            request.Content,
            request.ArticleId,
            request.AppUserId);

        if (!_predictionService.IsContentToxic(request.Content))
        {
            entity.SetApprovedStatus();
        }

        var result = await _commentRepository.AddAsync(entity, cancellationToken);

        return result.IsError
            ? result.Errors
            : new CreateCommentResponse();
    }
}
