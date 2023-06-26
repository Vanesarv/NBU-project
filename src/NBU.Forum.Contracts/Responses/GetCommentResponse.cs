namespace NBU.Forum.Contracts.Responses;

public class GetCommentResponse
{
    public string Content { get; set; } = default!;

    public string Author { get; set; } = default!;

    public string CreatedAt { get; set; } = default!;
}
