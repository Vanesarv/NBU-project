namespace NBU.Forum.Web;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddWeb(this WebApplicationBuilder builder)
    {
        builder
            .AddRoutingOptions()
            .AddRazorPages()
            .AddControllersWithViews();

        return builder;
    }

    private static WebApplicationBuilder AddControllersWithViews(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllersWithViews();

        return builder;
    }

    private static WebApplicationBuilder AddRazorPages(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();

        return builder;
    }

    private static WebApplicationBuilder AddRoutingOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        return builder;
    }
}
