namespace NBU.Forum.Infrastructure;

using Serilog;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Domain.AppUserAggregate;
using Configurations;
using Diagnostics;
using Persistence;
using System.Data;
using Microsoft.Extensions.Logging;
using Application.Services;
using Infrastructure.Services;
using Application.Articles;
using Infrastructure.Articles;
using Application.Comments;
using Infrastructure.Comments;
using Microsoft.Extensions.ML;
using Domain.Prediction;
using Application.Prediction;
using Infrastructure.Prediction;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder
            .AddConfigurations()
            .AddDbContext()
            .AddIdentity()
            .AddPersistence()
            .AddLogging()
            .AddDateTimeProvider()
            .AddHealthChecks()
            .AddPredictionEnginePool()
            .AddPredictionService();

        return builder;
    }

    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        using var services = app.Services.CreateScope();
        var dbContext = services.ServiceProvider.GetService<ForumDbContext>();

        dbContext!.Database.Migrate();

        return app;
    }

    public static WebApplication CreateRoles(this WebApplication app)
    {
        using var services = app.Services.CreateScope();
        var roleManager = services.ServiceProvider.GetService<RoleManager<IdentityRole>>();

        string[] roleNames = { "Admin", "Moderator" };

        foreach (var roleName in roleNames)
        {
            var roleExist = roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult();
            if (!roleExist)
            {
                var result = roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
            }
        }

        return app;
    }

    public static WebApplication CreateAdminUser(this WebApplication app)
    {
        using var services = app.Services.CreateScope();
        var userManager = services.ServiceProvider.GetService<UserManager<AppUser>>();

        var (adminUsername, adminEmail, adminPassword) = ("admin", "admin@admin.com", "admin");

        var adminExist = userManager!.FindByNameAsync(adminUsername).GetAwaiter().GetResult();

        if (adminExist is not null)
        {
            return app;
        }

        var admin = new AppUser()
        {
            UserName = adminUsername,
            Email = adminEmail
        };

        var result = userManager.CreateAsync(admin, adminPassword).GetAwaiter().GetResult();

        if (result.Succeeded)
        {
            userManager.AddToRoleAsync(admin, "Admin").GetAwaiter().GetResult();
        }

        return app;
    }

    private static WebApplicationBuilder AddConfigurations(this WebApplicationBuilder builder)
    {
        builder.Services.AddOptions<DatabaseConfiguration>()
            .BindConfiguration(nameof(DatabaseConfiguration))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services.AddOptions<DatabaseRetryPolicyConfiguration>()
            .BindConfiguration(nameof(DatabaseRetryPolicyConfiguration))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return builder;
    }

    private static WebApplicationBuilder AddDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ForumDbContext>((provider, options) =>
        {
            var databaseConfiguration = provider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;

            options.UseSqlServer(databaseConfiguration!.ConnectionString);
        });

        return builder;
    }

    private static WebApplicationBuilder AddIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequiredLength = 5;
        })
        .AddEntityFrameworkStores<ForumDbContext>();

        return builder;
    }

    private static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
        builder.Services.AddScoped<ICommentRepository, CommentRepository>();

        return builder;
    }

    private static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog((_, configuration) => configuration
            .WriteTo.Console()
            .Enrich.FromLogContext()
            .ReadFrom.Configuration(builder.Configuration));

        return builder;
    }

    private static WebApplicationBuilder AddDateTimeProvider(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        return builder;
    }

    private static WebApplicationBuilder AddHealthChecks(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IDbConnection>(provider =>
        {
            var databaseConfiguration = provider.GetRequiredService<IOptions<DatabaseConfiguration>>().Value;

            return new SqlConnection(databaseConfiguration!.ConnectionString);
        });

        builder.Services.AddHealthChecks()
            .AddCheck<DatabaseLivenessHealthCheck>(nameof(DatabaseLivenessHealthCheck),
                tags: new[] { "liveness" });

        return builder;
    }

    private static WebApplicationBuilder AddPredictionEnginePool(this WebApplicationBuilder builder)
    {
        string modelPath = Path.Combine(builder.Environment.ContentRootPath.Replace("Web", "Infrastructure"), "MLModel1.zip");
        builder.Services.AddPredictionEnginePool<ModelInput, ModelOutput>()
            .FromFile(modelPath);

        return builder;
    }

    private static WebApplicationBuilder AddPredictionService(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IPredictionService, PredictionService>();

        return builder;
    }
}