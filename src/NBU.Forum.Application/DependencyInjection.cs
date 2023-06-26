namespace NBU.Forum.Application;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using FluentValidation;
using MediatR;
using PipelineBehaviors;
using Mapster;
using MapsterMapper;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddApplication(this WebApplicationBuilder builder)
    {
        builder
            .AddMediator()
            .AddMediatorPipelineBehaviors()
            .AddFluentValidation()
            .AddMapping();

        return builder;
    }

    private static WebApplicationBuilder AddMediator(this WebApplicationBuilder builder)
    {
        builder.Services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });

        return builder;
    }

    private static WebApplicationBuilder AddMediatorPipelineBehaviors(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return builder;
    }

    private static WebApplicationBuilder AddFluentValidation(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return builder;
    }

    private static WebApplicationBuilder AddMapping(this WebApplicationBuilder builder)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(DependencyInjection).Assembly);
        builder.Services.TryAddSingleton(config);
        builder.Services.TryAddSingleton<IMapper, ServiceMapper>();

        return builder;
    }
}
