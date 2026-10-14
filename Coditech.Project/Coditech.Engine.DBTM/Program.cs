using Coditech.API.Common;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Registers common services.
/// </summary>
builder.RegisterCommonServices();

var nativeDllPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "NativePdfDll",
    "libwkhtmltox.dll"
);

if (!File.Exists(nativeDllPath))
{
    throw new FileNotFoundException("libwkhtmltox.dll not found", nativeDllPath);
}

var context = new CustomAssemblyLoadContext();
context.LoadUnmanagedLibrary(nativeDllPath);

/// Register your Helper services here
builder.Services.AddSingleton<IConverter>(
    new SynchronizedConverter(new PdfTools())
);
var app = builder.Build();

// Ensure forwarded headers are processed when running behind IIS/ARR or a TLS-terminating proxy.
// This preserves the original request scheme (https) and client IP so authentication and
// cookies behave correctly after enabling HTTPS in IIS.
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

class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public IntPtr LoadUnmanagedLibrary(string absolutePath)
        => LoadUnmanagedDll(absolutePath);

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        => LoadUnmanagedDllFromPath(unmanagedDllName);

    protected override Assembly Load(AssemblyName assemblyName)
        => null!;
}
