namespace NBU.Forum.Contracts.Responses;

public class GetUnapprovedCommentResponse
{
    public string CommentId { get; set; } = default!;

    public string ArticleId { get; set; } = default!;

    public string Content { get; set; } = default!;

    public string Author { get; set; } = default!;
}
