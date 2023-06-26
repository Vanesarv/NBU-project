namespace NBU.Forum.Application.Articles;

using Domain.ArticleAggregate;
using ErrorOr;
using Contracts.Responses;

public interface IArticleRepository
{
    public Task<IEnumerable<TProjection>> GetAsync<TProjection>(
        CancellationToken cancellationToken = default!) 
        where TProjection : class;

    public Task<ErrorOr<GetArticleByIdResponse>> GetByIdAsync(
        string articleId,
        CancellationToken cancellationToken = default!);

    public Task<ErrorOr<Article>> AddAsync(
        Article article,
        CancellationToken cancellationToken = default!);
}
