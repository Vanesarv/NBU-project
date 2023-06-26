namespace NBU.Forum.Infrastructure.Diagnostics;

using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

public class DatabaseLivenessHealthCheck : IHealthCheck
{
    private readonly IDbConnection _dbConnection;
    private readonly ILogger _logger;

    public DatabaseLivenessHealthCheck(
        IDbConnection dbConnection,
        ILogger logger)
    {
        _dbConnection = dbConnection;
        _logger = logger.ForContext<DatabaseLivenessHealthCheck>();
    }
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            using (_dbConnection)
            {
                _dbConnection.Open();
                using var command = _dbConnection.CreateCommand();
                command.CommandText = "SELECT 1 FROM dbo.AspNetUsers LIMIT 1";
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            _logger.Error(ex, 
                "Database healthcheck failed.");

            return Task.FromResult(new HealthCheckResult(HealthStatus.Unhealthy,
                exception: ex));
        }

        return Task.FromResult(HealthCheckResult.Healthy());
    }
}
