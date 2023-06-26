namespace NBU.Forum.Domain.Shared;

public interface IAuditEntity
{
    public DateTime CreatedAt { get; set; }

    public DateTime LastUpdatedAt { get; set; }
}
