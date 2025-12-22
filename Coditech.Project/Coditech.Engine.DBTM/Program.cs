using Coditech.API.Common;
using DinkToPdf;
using DinkToPdf.Contracts;
using System.Reflection;
using System.Runtime.Loader;

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
