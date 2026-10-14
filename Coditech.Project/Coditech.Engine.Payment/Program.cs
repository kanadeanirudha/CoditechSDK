using Coditech.API.Common;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Registers common services.
/// </summary>
builder.RegisterCommonServices();

var app = builder.Build();

// Ensure forwarded headers are processed when running behind IIS/ARR or a TLS-terminating proxy.
// This preserves the original request scheme (https) and client IP so authentication and
// cookies behave correctly after enabling HTTPS in IIS, while still working for plain HTTP.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// Process forwarded headers early in the pipeline.
app.UseForwardedHeaders();

// Fallback: respect X-Forwarded-Proto header for request scheme if present.
app.Use(async (context, next) =>
{
    if (context.Request.Headers.TryGetValue("X-Forwarded-Proto", out var proto))
    {
        context.Request.Scheme = proto.ToString();
    }
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/// <summary>
/// Registers application services with the specified builder.
/// </summary>
app.RegisterApplicationServices(builder);

app.Run();
