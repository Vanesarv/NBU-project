namespace NBU.Forum.Application.Articles.Queries.GetArticleById;

using MediatR;
using ErrorOr;
using Contracts.Responses;

public sealed class GetArticleByIdQuery : IRequest<ErrorOr<GetArticleByIdResponse>>
{
	public GetArticleByIdQuery(string articleId)
		=> ArticleId = articleId;

    public string ArticleId { get; }
}
