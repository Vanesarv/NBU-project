using NBU.Forum.Web;
using NBU.Forum.Application;
using NBU.Forum.Infrastructure;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddWeb()
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();

app.UseExceptionHandler("/error");
app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHealthChecks("/liveness",
    new HealthCheckOptions { Predicate = healthcheck => healthcheck.Tags.Contains("liveness") });

app
    .MigrateDatabase()
    .CreateRoles()
    .CreateAdminUser();

app.Run();
