using Coditech.Admin;

/// <summary>
/// Creates a WebApplication Builder with the given arguments.
/// </summary>
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Registers common services.
/// </summary>
builder.RegisterCommonServices();

// When hosting behind IIS/ARR or another proxy that terminates TLS, the incoming
// requests to the app can appear as HTTP. Ensure forwarded headers are processed
// so the application sees the original scheme (https) and client IP. This fixes
// issues where authentication cookies or redirects are incorrect after enabling
// HTTPS in IIS.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    // Clear the default known networks/proxies so forwarded headers from IIS/ARR are accepted.
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

/// <summary>
/// Builds the application.
/// </summary>
var app = builder.Build();

// Process forwarded headers early in the pipeline so downstream middleware
// (authentication, cookie handling, etc.) sees the correct scheme.
app.UseForwardedHeaders();

// As an extra safeguard, if a proxy uses X-Forwarded-Proto but forwarded headers
// middleware isn't applied for some reason, adjust the request scheme from header.
app.Use(async (context, next) =>
{
    if (context.Request.Headers.TryGetValue("X-Forwarded-Proto", out var proto))
    {
        context.Request.Scheme = proto.ToString();
    }
    await next();
});
/// <summary>
/// Registers application services with the specified builder.
/// </summary>
app.RegisterApplicationServices(builder);
/// <summary>
/// Executes the application's startup logic. 
/// </summary>
app.Run();


