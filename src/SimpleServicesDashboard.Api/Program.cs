using Asp.Versioning.ApiExplorer;
using Hellang.Middleware.ProblemDetails;
using Serilog;
using SimpleServicesDashboard.Api.Infrastructure.Extensions;
using SimpleServicesDashboard.Common.Extensions;

var builder = WebApplication.CreateBuilder();

// configure Serilog for logging
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});

// configure application services
ConfigureServices(builder);

// create the app to configure the middleware
var app = builder.Build();

// configure the web app middleware components
var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
ConfigureApplication(app, builder.Environment, apiVersionDescriptionProvider);

// run the application
await app.RunAsync();
return;

// Configure the DI and all services on the web application

void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.ConfigureApiService(builder.Configuration, builder.Environment, true);
}

// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
void ConfigureApplication(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiProvider)
{
    var healthCheckConfig = builder.Configuration.GetHealthCheckConfiguration();

    // configure Forwarder headers for proxies and Load Balancers
    app.ConfigureForwarderOptions();

    if (!env.IsEnvironment("Local"))
    {
        // custom configuration for security headers (HSTS for 60 days)
        app.ConfigureSecurityHeaders();
    }

    // redirect to the HTTPS connection
    app.UseHttpsRedirection();

    // Add using ProblemDetail middleware to handle errors and use RFC7807 standard
    app.UseProblemDetails();

    // configure Swagger UI
    app.ConfigureSwagger(apiProvider);

    // add logger for all requests in the web server
    app.ConfigureSerilog();

    // use default files
    app.UseDefaultFiles();

    // allow to use static files
    app.UseStaticFiles();

    // Use routing middleware to handle requests to the controllers
    app.UseRouting();

    app.UseEndpoints(endpoints =>
    {
        // add controllers endpoints
        endpoints.MapControllers();

        // add routing for the Razor pages
        endpoints.MapRazorPages();

        // add health checks endpoints and configurations
        endpoints.AddHealthCheckEndpoints(healthCheckConfig);
    });
}