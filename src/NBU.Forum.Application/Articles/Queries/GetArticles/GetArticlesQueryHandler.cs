namespace NBU.Forum.Application.Articles.Queries.GetArticles;

using MediatR;
using Contracts.Responses;
using System.Threading;
using System.Threading.Tasks;

public sealed class GetArticlesQueryHandler : IRequestHandler<GetArticlesQuery,
    IEnumerable<GetArticleResponse>>
{
    private readonly IArticleRepository _articleRepository;

    public GetArticlesQueryHandler(IArticleRepository articleRepository)
        => _articleRepository = articleRepository;

    public async Task<IEnumerable<GetArticleResponse>> Handle(GetArticlesQuery request, CancellationToken cancellationToken)
    {
        var articles = await _articleRepository.GetAsync<GetArticleResponse>(cancellationToken);

        return articles;
    }
}
