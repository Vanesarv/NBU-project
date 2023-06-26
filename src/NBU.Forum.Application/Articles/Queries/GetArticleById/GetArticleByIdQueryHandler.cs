namespace NBU.Forum.Application.Articles.Queries.GetArticleById;

using ErrorOr;
using MediatR;
using Contracts.Responses;

public sealed class GetArticleByIdQueryHandler : IRequestHandler<GetArticleByIdQuery,
    ErrorOr<GetArticleByIdResponse>>
{
    private readonly IArticleRepository _articleRepository;

    public GetArticleByIdQueryHandler(IArticleRepository articleRepository)
        => _articleRepository = articleRepository;

    public async Task<ErrorOr<GetArticleByIdResponse>> Handle(GetArticleByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _articleRepository.GetByIdAsync(request.ArticleId,
            cancellationToken);

        return result;
    }
}
