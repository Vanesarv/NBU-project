namespace NBU.Forum.Application.Articles.Queries.GetArticles;

using MediatR;
using NBU.Forum.Contracts.Responses;

public sealed class GetArticlesQuery : IRequest<IEnumerable<GetArticleResponse>>
{}