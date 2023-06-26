namespace NBU.Forum.Contracts.Requests;

public class CreateArticleRequest
{
    public string Title { get; set; } = default!;

    public string Content { get; set; } = default!;
}
