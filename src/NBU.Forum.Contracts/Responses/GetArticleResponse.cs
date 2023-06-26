namespace NBU.Forum.Contracts.Responses;

public class GetArticleResponse
{
    public string ArticleId { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Author { get; set; } = default!;
}
