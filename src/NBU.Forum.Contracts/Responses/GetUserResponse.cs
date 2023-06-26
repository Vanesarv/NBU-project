namespace NBU.Forum.Contracts.Responses;

public class GetUserResponse
{
    public string UserId { get; set; } = default!;

    public string Username { get; set; } = default!;

    public string Email { get; set; } = default!;
}
