using Coditech.API.Common;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using System.Runtime.Loader;

var builder = WebApplication.CreateBuilder(args);

/// <summary>
/// Registers common services.
/// </summary>
builder.RegisterCommonServices();


 ///  LOAD wkhtmltopdf DLL

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


app.UseStaticFiles(); 

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "data")),
    RequestPath = "/data"
});

app.UseRouting();
 
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

 ///  ROUTING & MVC

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.RegisterApplicationServices(builder);

app.Run();


  /// <summary>
  /// Custom DLL Loader
  /// </summary>

class CustomAssemblyLoadContext : AssemblyLoadContext
{
    public IntPtr LoadUnmanagedLibrary(string absolutePath)
        => LoadUnmanagedDllFromPath(absolutePath);

    protected override Assembly Load(AssemblyName assemblyName)
        => null!;
}
