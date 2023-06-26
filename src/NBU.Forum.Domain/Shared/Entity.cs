namespace NBU.Forum.Domain.Shared;

public abstract class Entity
{
    public string Id { get; private set; } = Guid.NewGuid().ToString();
}
