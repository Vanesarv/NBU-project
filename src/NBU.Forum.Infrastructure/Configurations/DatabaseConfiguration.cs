namespace NBU.Forum.Infrastructure.Configurations;

using System.ComponentModel.DataAnnotations;

internal sealed class DatabaseConfiguration
{
    [Required]
    public string ConnectionString { get; set; } = default!;
}
