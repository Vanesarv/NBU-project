namespace NBU.Forum.Application.Articles.Commands.CreateArticle;

using MediatR;
using Contracts.Responses;
using ErrorOr;
using System.Threading;
using System.Threading.Tasks;
using Domain.ArticleAggregate;

public sealed class CreateArticleCommandHandler : IRequestHandler<CreateArticleCommand,
    ErrorOr<CreateArticleResponse>>
{
    private readonly IArticleRepository _articleRepository;

    public CreateArticleCommandHandler(IArticleRepository articleRepository)
        => _articleRepository = articleRepository;

    public async Task<ErrorOr<CreateArticleResponse>> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
    {
        var entity = Article.Create(
            request.Title,
            request.Content,
            request.AppUserId);

        var result = await _articleRepository.AddAsync(entity, cancellationToken);

        return result.IsError
            ? result.Errors
            : new CreateArticleResponse(result.Value.Id);
    }
}
