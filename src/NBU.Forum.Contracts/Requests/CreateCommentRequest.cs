namespace NBU.Forum.Contracts.Requests;

public class CreateCommentRequest
{
    public string ArticleId { get; set; } = default!;

    public string Content { get; set; } = default!;
}
