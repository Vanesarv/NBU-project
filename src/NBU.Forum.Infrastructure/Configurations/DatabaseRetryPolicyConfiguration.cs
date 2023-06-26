namespace NBU.Forum.Infrastructure.Configurations;

using System.ComponentModel.DataAnnotations;

internal sealed class DatabaseRetryPolicyConfiguration
{
    [Required]
    public TimeSpan Delay { get; set; }

    [Required]
    public int RetryCount { get; set; }
}
