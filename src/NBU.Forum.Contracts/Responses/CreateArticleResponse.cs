namespace NBU.Forum.Contracts.Responses;

public class CreateArticleResponse
{
	public CreateArticleResponse(string articleId)
		=> ArticleId = articleId;

    public string ArticleId { get; } = default!;
}
