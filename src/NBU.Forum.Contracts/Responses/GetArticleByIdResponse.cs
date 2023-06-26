namespace NBU.Forum.Contracts.Responses;

public class GetArticleByIdResponse
{
    public string ArticleId { get; set; } = default!;

    public string Author { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Content { get; set; } = default!;

    public IEnumerable<GetCommentResponse> Comments { get; set; }
}
